using System;
using System.Web.Mvc;
using StructureMap;

namespace Centro.MVC.Controllers
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(Type controllerType)
        {
            if (controllerType == null)
                return null;
            return (IController)ObjectFactory.GetInstance(controllerType);
        }
    }
}
