using Microsoft.AspNetCore.Mvc.Filters;

namespace api_catalogo.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("#### Executando -> OnActionExecuting");
            _logger.LogInformation("######################################");
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("######################################");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("#### Executando -> OnActionExecuted");
            _logger.LogInformation("######################################");
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
            _logger.LogInformation("######################################");
        }

    }
}
