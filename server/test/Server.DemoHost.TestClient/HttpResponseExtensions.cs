using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;

namespace Server.DemoHost.TestClient;

public static class HttpResponseExtensions
{
    public static async Task<T> GetData<T>(this Task<IHttpResponse<IResult<T>>> task)
    {
        var response = await task;

        return response.Data.Data;
    }

    public static async Task<IReadOnlyCollection<T>> GetData<T>(this Task<IHttpResponse<IResult<IEnumerable<T>>>> task)
    {
        var response = await task;

        return response.Data.Data.ToArray();
    }

    public static async Task<IResult<T>> GetResult<T>(this Task<IHttpResponse<IResult<T>>> task)
    {
        var response = await task;

        return response.Data;
    }
}