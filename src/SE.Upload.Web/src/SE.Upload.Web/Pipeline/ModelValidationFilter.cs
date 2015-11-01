// ==========================================================================
// ModelValidationFilter.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace SE.Upload.Web.Pipeline
{
    public sealed class ModelValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
