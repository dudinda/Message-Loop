using System.Reflection;

using MessageLoop.Web.Common.Controllers.v1;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MessageLoop.Web.Common.Code.Mvc
{
    public class MessageControllerConvention<TEnum> : IControllerModelConvention where TEnum : Enum
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType == typeof(MessageController<TEnum>).GetTypeInfo())
            {
                controller.ControllerName = typeof(TEnum).Name;
                var segment = controller.ControllerName.ToLower();
                foreach (var selector in controller.Selectors)
                {
                    selector.AttributeRouteModel?.Template = string.Format(
                        selector.AttributeRouteModel?.Template, segment);

                }
            }
        }
    }
}
