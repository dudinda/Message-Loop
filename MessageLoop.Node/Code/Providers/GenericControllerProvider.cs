using System.Reflection;

using MessageLoop.Web.Common.Code.Enums;
using MessageLoop.Web.Common.Controllers.v1;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MessageLoop.Node.Code.Providers
{
    public class GenericControllerProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(
            IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            feature.Controllers.Add(typeof(MessageController<Messages>).GetTypeInfo());
        }
    }
}
