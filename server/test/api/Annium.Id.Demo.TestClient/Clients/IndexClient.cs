using System;
using System.Threading.Tasks;
using Annium.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Demo.TestClient.Clients
{
    public class IndexClient : ClientBase
    {
        public IndexClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IActionResult>> Base(
        )
        {
            return await Request.Clone()
                .Get("base")
                .AsResponseAsync<IActionResult>();
        }

        public async Task<IHttpResponse<IActionResult>> IsAdmin(
        )
        {
            return await Request.Clone()
                .Get("isAdmin")
                .AsResponseAsync<IActionResult>();
        }

        public async Task<IHttpResponse<IActionResult>> HasPaymentsAccess(
        )
        {
            return await Request.Clone()
                .Get("hasPaymentsAccess")
                .AsResponseAsync<IActionResult>();
        }

        public async Task<IHttpResponse<IActionResult>> HasCompanyPaymentsAccess(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"hasCompanyPaymentsAccess/{companyId}")
                .AsResponseAsync<IActionResult>();
        }
    }
}