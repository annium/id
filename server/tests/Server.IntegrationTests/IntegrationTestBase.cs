using System;
using System.IO;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Annium.Infrastructure.Hosting;
using Annium.Net.Http;
using Annium.Net.Mail.Testing;
using Annium.Testing;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using Server.DemoHost;
using Server.DemoHost.TestClient.Clients;
using Server.Host;
using Server.Host.TestClient.Clients;
using Xunit;
using Database = Server.IntegrationTests.Fixtures.Database;
using DemoApiClient = Server.DemoHost.TestClient.Clients.DemoClient;
using ServicePack = Server.DemoHost.ServicePack;

namespace Server.IntegrationTests;

/// <summary>
/// Base for the integration tests, owning the in-memory hosts the clients talk to.
/// </summary>
/// <remarks>
/// The harness models a host as an object with a lifecycle (<see cref="TestHostBase{TEntryPoint}"/>)
/// rather than a factory method, so the id host is started once per test and the demo host - whose
/// audience is baked into its auth options at registration - is started per audience by
/// <see cref="DemoAsync(Guid)"/>. That is why the demo accessors are async and the id ones are not.
/// </remarks>
public class IntegrationTestBase : TestBase
{
    /// <summary>Fake data generator shared by the tests.</summary>
    protected Faker Faker { get; } = new();

    /// <summary>Email sink registered into the id host, so tests can read what it sent.</summary>
    protected readonly TestEmailService EmailService = new();

    /// <summary>The id host, started by <see cref="InitializeAsync"/>.</summary>
    private IdTestHost _idHost = null!;

    /// <summary>The demo host, started on first use and rebuilt per audience.</summary>
    private DemoTestHost? _demoHost;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
    /// </summary>
    /// <param name="outputHelper">xUnit output helper the hosts and the test container log through.</param>
    public IntegrationTestBase(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        // TestBase freezes registrations once InitializeAsync starts, so both factories are declared
        // here, against clients built lazily - the hosts they read are started later, in the test body
        Register(container =>
        {
            container.AddHttpRequestFactory("id", (_, _) => _idHost.Server.CreateClient(), isDefault: true);
            container.AddHttpRequestFactory("demo", (_, _) => _demoHost!.Server.CreateClient());
        });
    }

    /// <summary>
    /// Acquires the shared database container and starts the id host.
    /// </summary>
    /// <returns>A task that completes once the host is ready to serve requests.</returns>
    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        await Database.AcquireAsync();

        _idHost = new IdTestHost(OutputHelper, EmailService);
        await _idHost.StartAsync();
    }

    /// <summary>
    /// Tears down both hosts before the test container itself is disposed.
    /// </summary>
    /// <returns>A task that completes once every host has been disposed.</returns>
    public override async ValueTask DisposeAsync()
    {
        if (_demoHost is not null)
            await _demoHost.DisposeAsync();

        if (_idHost is not null)
            await _idHost.DisposeAsync();

        await base.DisposeAsync();
    }

    #region id

    /// <summary>
    /// Builds an anonymous client against the id host.
    /// </summary>
    /// <returns>The client.</returns>
    protected ExtendedClient Id() => IdApi.ApiClient(EmailService);

    /// <summary>
    /// Builds an authorized client against the id host.
    /// </summary>
    /// <param name="token">The bearer token to send.</param>
    /// <returns>The client.</returns>
    protected ExtendedClient Id(string token) => IdApi.BearerAuthorization(token).ApiClient(EmailService);

    /// <summary>Gets a fresh request against the id host.</summary>
    private IHttpRequest IdApi => GetKeyed<IHttpRequestFactory>("id").New();

    #endregion

    #region demo

    /// <summary>
    /// Starts a demo host for the given audience and builds an anonymous client against it.
    /// </summary>
    /// <param name="appId">The app the host accepts tokens for.</param>
    /// <returns>The client.</returns>
    protected async Task<DemoApiClient> DemoAsync(Guid appId) => (await DemoApiAsync(appId)).DemoClient();

    /// <summary>
    /// Starts a demo host for the given audience and builds an authorized client against it.
    /// </summary>
    /// <param name="appId">The app the host accepts tokens for.</param>
    /// <param name="token">The bearer token to send.</param>
    /// <returns>The client.</returns>
    protected async Task<DemoApiClient> DemoAsync(Guid appId, string token) =>
        (await DemoApiAsync(appId)).BearerAuthorization(token).DemoClient();

    /// <summary>
    /// Starts the demo host bound to <paramref name="appId"/> and returns a request against it.
    /// </summary>
    /// <param name="appId">The app the host accepts tokens for.</param>
    /// <returns>A fresh request against the demo host.</returns>
    private async Task<IHttpRequest> DemoApiAsync(Guid appId)
    {
        if (_demoHost is not null)
            await _demoHost.DisposeAsync();

        _demoHost = new DemoTestHost(OutputHelper, appId);
        await _demoHost.StartAsync();

        return GetKeyed<IHttpRequestFactory>("demo").New();
    }

    #endregion
}

/// <summary>
/// The id server, hosted in memory with the test service pack and the test email sink.
/// </summary>
internal class IdTestHost : TestHostBase<Api>
{
    /// <summary>The email sink the test reads from.</summary>
    private readonly TestEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdTestHost"/> class.
    /// </summary>
    /// <param name="outputHelper">xUnit output helper the host logs through.</param>
    /// <param name="emailService">The email sink to register in place of the real one.</param>
    public IdTestHost(ITestOutputHelper outputHelper, TestEmailService emailService)
        : base(outputHelper)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Applies the test service pack and points the host at the throwaway database.
    /// </summary>
    /// <param name="builder">The host builder.</param>
    protected override void ConfigureHost(IHostBuilder builder)
    {
        builder.UseServicePack<TestServicePack>();
        builder.ConfigureServices(services =>
        {
            var container = new ServiceContainer(services);
            container.Add(_emailService).AsSelf().AsInterfaces().Singleton();
            container.AddConfiguration(Database.Config);
        });
    }
}

/// <summary>
/// The demo server, hosted in memory and accepting tokens for a single audience.
/// </summary>
internal class DemoTestHost : TestHostBase<Demo>
{
    /// <summary>The audience this host validates tokens against.</summary>
    private readonly Guid _appId;

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoTestHost"/> class.
    /// </summary>
    /// <param name="outputHelper">xUnit output helper the host logs through.</param>
    /// <param name="appId">The app whose tokens this host accepts.</param>
    public DemoTestHost(ITestOutputHelper outputHelper, Guid appId)
        : base(outputHelper)
    {
        _appId = appId;
    }

    /// <summary>
    /// Applies the demo service pack and wires id authorization to this host's audience.
    /// </summary>
    /// <param name="builder">The host builder.</param>
    protected override void ConfigureHost(IHostBuilder builder)
    {
        builder.UseServicePack<ServicePack>();
        builder.ConfigureServices(services =>
        {
            new ServiceContainer(services).AddIdAuthorization(
                (_, options) =>
                {
                    options.Audience = _appId;
                    options.PublicKeyFile = Path.Combine("keys", "public.key");
                    options.AccessTokenLifeTime = Duration.FromMinutes(5);
                    options.RefreshTokenLifeTime = Duration.FromMinutes(5);
                }
            );
        });
    }
}
