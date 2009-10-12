using System;
using System.Web.Mvc;
using StructureMap;

namespace Centro.Web.Mvc.Controllers
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(Type controllerType)
        {
            if (controllerType == null)
                return base.GetControllerInstance(controllerType);
            return (IController)ObjectFactory.GetInstance(controllerType) ??
                   base.GetControllerInstance(controllerType);
        }
    }
}
