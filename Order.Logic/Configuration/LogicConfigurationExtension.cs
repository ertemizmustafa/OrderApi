using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Order.Logic.Behaviours;

namespace Order.Logic.Configuration
{
    public static class LogicConfigurationExtension
    {
        public static IServiceCollection AddOrdersLogic(this IServiceCollection serviceCollection)
        {
            //Mediator logic katmanı log
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
            //Mediator logic exception log 
            serviceCollection.AddScoped(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionPipelineBehaviour<,,>));
           
            return serviceCollection;
        }
    }
}
