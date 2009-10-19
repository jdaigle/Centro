using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Centro.Validation;

namespace Centro.Web.Mvc
{
    public static class ViewModelExtensions
    {
        public static void Validate(this IValidatable viewModel, ModelStateDictionary modelState, string prefix)
        {
            var fixedPrefix = prefix ?? string.Empty;
            if (!fixedPrefix.EndsWith("."))
                fixedPrefix += ".";
            foreach (var error in viewModel.ValidationErrors())
            {
                var key = fixedPrefix + error.PropertyName;
                modelState.AddModelError(key, error.ErrorMessage);
            }
        }
    }
}
