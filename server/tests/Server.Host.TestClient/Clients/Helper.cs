using System.Threading;
using Bogus;

namespace Server.Host.TestClient.Clients;

internal static class Helper
{
    /// <summary>
    /// A <see cref="Faker"/> per thread. xUnit runs test classes in parallel and they all reach for
    /// this, and Bogus is not thread-safe: concurrent callers corrupt the shared Randomizer and start
    /// handing out colliding values, which then surface as unrelated failures deep in the server.
    /// </summary>
    private static readonly ThreadLocal<Faker> _faker = new(() => new Faker());

    public static Faker Faker => _faker.Value!;
}
