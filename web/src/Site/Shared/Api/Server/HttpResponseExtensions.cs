using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;

namespace Site.Shared.Api.Server;

public static class HttpResponseExtensions
{
    public static async Task<T> GetData<T>(this Task<IResult<T>> task)
    {
        var response = await task;

        return response.Data;
    }

    public static async Task<IReadOnlyCollection<T>> GetData<T>(this Task<IResult<IEnumerable<T>>> task)
    {
        var response = await task;

        return response.Data.ToArray();
    }
}