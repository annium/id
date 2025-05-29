using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;

namespace Server.Host.TestClient.Clients;

public static class HttpResponseExtensions
{
    public static async Task<T> GetDataAsync<T>(this Task<IHttpResponse<IResult<T>>> task)
    {
#pragma warning disable VSTHRD003
        var response = await task;
#pragma warning restore VSTHRD003

        return response.Data.Data;
    }

    public static async Task<IReadOnlyCollection<T>> GetDataAsync<T>(
        this Task<IHttpResponse<IResult<IEnumerable<T>>>> task
    )
    {
#pragma warning disable VSTHRD003
        var response = await task;
#pragma warning restore VSTHRD003

        return response.Data.Data.ToArray();
    }

    public static async Task<IResult<T>> GetResultAsync<T>(this Task<IHttpResponse<IResult<T>>> task)
    {
#pragma warning disable VSTHRD003
        var response = await task;
#pragma warning restore VSTHRD003

        return response.Data;
    }
}
