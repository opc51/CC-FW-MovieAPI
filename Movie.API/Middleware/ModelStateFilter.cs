using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Movie.API.Middleware
{
    /// <summary>
    /// Created to allow FluentValidation to work automatically in controller actions
    /// 
    /// Makes for cleaner controllers.Has to be injected into <see cref="Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)"/>
    /// </summary>
    public class ModelStateFilter : IActionFilter
    {
        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
