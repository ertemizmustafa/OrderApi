using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
namespace Order.Logic.Behaviours
{
    public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingPipelineBehaviour(ILogger<TRequest> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = $"{typeof(TRequest).Name} [{Guid.NewGuid()}]";

            TResponse response;
            var stopWatch = Stopwatch.StartNew();
            _logger.LogInformation($"[START] {requestName}.");

            try
            {
                try
                {
                    _logger.LogInformation($"[PROPERTIES] {requestName} {JsonSerializer.Serialize(request)}.");
                }
                catch
                {
                    _logger.LogInformation($"[SERIALIZATION ERROR] {requestName} Could not serialize request.");
                }

                response = await next();
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation($"[END] {requestName} ExecutionTime = {stopWatch.ElapsedMilliseconds}ms.");
            }

            return response;
        }
    }
}
