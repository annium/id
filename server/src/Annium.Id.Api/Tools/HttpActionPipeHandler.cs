using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Extensions.Primitives;
using Annium.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Annium.Id.Api.Tools
{
    public class HttpActionPipeHandler<TRequest, TResponse> : IPipeRequestHandler<ValueTuple<ModelStateDictionary, TRequest>, TRequest, IStatusResult<HttpStatusCode, TResponse>, IStatusResult<HttpStatusCode, TResponse>>
    {
        private readonly ILogger<HttpActionPipeHandler<TRequest, TResponse>> logger;

        public HttpActionPipeHandler(
            ILogger<HttpActionPipeHandler<TRequest, TResponse>> logger
        )
        {
            this.logger = logger;
        }

        public Task<IStatusResult<HttpStatusCode, TResponse>> HandleAsync(
            ValueTuple<ModelStateDictionary, TRequest> payload,
            CancellationToken cancellationToken,
            Func<TRequest, Task<IStatusResult<HttpStatusCode, TResponse>>> next
        )
        {
            var(modelState, request) = payload;
            if (!modelState.IsValid)
            {
                logger.Trace($"Model of {typeof(TRequest).Name} is not valid");
                return Task.FromResult(GetBadRequestResult(modelState));
            }

            return next(request);
        }

        private IStatusResult<HttpStatusCode, TResponse> GetBadRequestResult(ModelStateDictionary modelState)
        {
            var result = Result.New<HttpStatusCode, TResponse>(HttpStatusCode.BadRequest, default(TResponse));

            foreach (var(field, entry) in modelState)
            {
                var label = field.CamelCase();
                foreach (var error in entry.Errors)
                    result.Error(label, error.ErrorMessage);
            }

            return result;
        }
    }
}