using System;
using System.Threading.Tasks;
using Annium.Id.Demo.ViewModels;
using Annium.Net.Http;

namespace Annium.Id.Demo.TestClient.Clients
{
    public class IndexClient : ClientBase
    {
        public IndexClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IdTokenResponse>> Base(
        )
        {
            return await Request.Clone()
                .Get("base")
                .AsResponseAsync<IdTokenResponse>();
        }

        public async Task<IHttpResponse<IdTokenResponse>> IsAdmin(
        )
        {
            return await Request.Clone()
                .Get("isAdmin")
                .AsResponseAsync<IdTokenResponse>();
        }

        public async Task<IHttpResponse<IdTokenResponse>> HasPaymentsAccess(
        )
        {
            return await Request.Clone()
                .Get("hasPaymentsAccess")
                .AsResponseAsync<IdTokenResponse>();
        }

        public async Task<IHttpResponse<IdTokenResponse>> HasCompanyPaymentsAccess(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"hasCompanyPaymentsAccess/{companyId}")
                .AsResponseAsync<IdTokenResponse>();
        }
    }
}