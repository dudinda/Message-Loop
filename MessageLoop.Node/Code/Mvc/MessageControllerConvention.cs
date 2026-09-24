using System.Reflection;

using MessageLoop.Web.Common.Code.Enums;
using MessageLoop.Web.Common.Controllers.v1;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MessageLoop.Node.Code.Mvc
{
    public class MessageControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType == typeof(MessageController<Messages>).GetTypeInfo())
            {
                controller.ControllerName = nameof(Messages);
                var segment = controller.ControllerName.ToLower();
                foreach (var selector in controller.Selectors)
                {
                    selector.AttributeRouteModel.Template += $"/{segment}";

                }
            }
        }
    }
}
