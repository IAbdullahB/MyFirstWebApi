using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyFirstWebAPI.Middlewares.Filters
{
    public class LogActivityFilter : IActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;
        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing action: {context.ActionDescriptor.DisplayName} " +
                                   $"on controller: {context.Controller.GetType().Name}" + 
                                   $" with arguments: {JsonSerializer.Serialize(context.ActionArguments)}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action: {context.ActionDescriptor.DisplayName} " +
                                   $"finished execution on controller: {context.Controller.GetType().Name}");
        }
    }
}
