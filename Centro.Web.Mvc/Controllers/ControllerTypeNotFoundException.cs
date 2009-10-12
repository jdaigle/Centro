using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Centro.MVC.Controllers
{
    public class ControllerTypeNotFoundException : HttpException
    {
        public ControllerTypeNotFoundException(string controllerName, string requestedUri)
            : base(404, string.Format("Could not find a type for the controller name '{0}' for requested uri '{1}'",controllerName, requestedUri))
        {
        }
    }
}
