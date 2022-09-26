using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace FileUploadApp.Core.Mvc;

[AttributeUsage(AttributeTargets.Method)]
public class FormContentTypeAttribute : Attribute, IActionConstraint
{
    public int Order => 0;

    public bool Accept(ActionConstraintContext ctx) =>
        ctx.RouteContext.HttpContext.Request.HasFormContentType;
}
