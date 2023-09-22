using Annium.Net.Http;

namespace Site.Shared.Api.Server.Clients;

public class Root
{
    public AppClient App => new(_request);
    public ClaimClient Claim => new(_request);
    public CompanyClaimClient CompanyClaim => new(_request);
    public CompanyClient Company => new(_request);
    public CompanyRoleClient CompanyRole => new(_request);
    public CompanyUserClient CompanyUser => new(_request);
    public LoginClient Login => new(_request);
    public MeClient Me => new(_request);
    public RoleClient Role => new(_request);
    public UserClient User => new(_request);
    private readonly IHttpRequest _request;

    internal Root(IHttpRequest request)
    {
        _request = request;
    }
}