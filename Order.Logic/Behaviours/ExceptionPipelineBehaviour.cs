using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Logic.Behaviours
{
    public class ExceptionPipelineBehaviour<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException> where TException : Exception
    {

        private readonly ILogger<TRequest> _logger;

        public ExceptionPipelineBehaviour(ILogger<TRequest> logger) => _logger = logger;

        public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            _logger.LogError($"[EXCEPTION] {request} \nExceptionMessage : {exception.Message} \nExceptionStackTrace: {exception.StackTrace}");

            return Task.CompletedTask;
        }
    }
}
