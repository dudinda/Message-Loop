using System.Reflection;

using MessageLoop.Web.Common.Controllers.v1;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MessageLoop.Web.Common.Code.Mvc
{
    public class MessageControllerProvider<TEnum> : IApplicationFeatureProvider<ControllerFeature> where TEnum : Enum
    {
        public void PopulateFeature(
            IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            feature.Controllers.Add(typeof(MessageController<TEnum>).GetTypeInfo());
        }
    }
}
