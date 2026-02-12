using MessageLoop.Common.Models.LongRun;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace MessageLoop.Web.Common.Code.Filters
{
    public class LongRunAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public string Description { get; set; }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var svc = context.HttpContext.RequestServices;
            var ctx = svc.GetService<LongRunContext>();
            ctx.Id.Value = Guid.NewGuid().ToString();
            ctx.Description.Value = Description;
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
