using Annium.Net.Http;

namespace Server.Host.TestClient.Clients;

public class Root
{
    public AppClient App { get; }
    public ClaimClient Claim { get; }
    public CompanyClaimClient CompanyClaim { get; }
    public CompanyClient Company { get; }
    public CompanyRoleClient CompanyRole { get; }
    public CompanyUserClient CompanyUser { get; }
    public LoginClient Login { get; }
    public MeClient Me { get; }
    public RoleClient Role { get; }
    public UserClient User { get; }

    public Root(IHttpRequest request)
    {
        App = new AppClient(request);
        Claim = new ClaimClient(request);
        CompanyClaim = new CompanyClaimClient(request);
        Company = new CompanyClient(request);
        CompanyRole = new CompanyRoleClient(request);
        CompanyUser = new CompanyUserClient(request);
        Login = new LoginClient(request);
        Me = new MeClient(request);
        Role = new RoleClient(request);
        User = new UserClient(request);
    }
}