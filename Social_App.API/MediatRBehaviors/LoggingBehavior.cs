using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_App.API.MediatRBehaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation($"[Start] Handle request={typeof(TRequest).Name} - response={typeof(TRequest).Name} and details => {request}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var response = await next();
            stopWatch.Stop();
            if (stopWatch.Elapsed.Seconds > 3)
                logger.LogInformation($"[Performance] the request {typeof(TRequest).Name} take {stopWatch.Elapsed.Seconds} seconds to finish");

            logger.LogInformation(message: $"[End] Handle request={typeof(TRequest).Name} - response={typeof(TRequest).Name} and details => {request}");

            return response;
        }
    }
}
