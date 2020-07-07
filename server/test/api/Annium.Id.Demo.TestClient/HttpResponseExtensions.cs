using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;

namespace Annium.Id.Demo.TestClient
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> GetData<T>(this Task<IHttpResponse<IResult<T>>> task)
        {
            var response = await task;

            return response.Data.Data;
        }
    }
}