using System;

using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace FileUploadApp.Application.Common.Mvc;

[AttributeUsage(AttributeTargets.Method)]
public class FormContentTypeAttribute : Attribute, IActionConstraint
{
    public int Order => 0;

    public bool Accept(ActionConstraintContext context) =>
        context.RouteContext.HttpContext.Request.HasFormContentType;
}
