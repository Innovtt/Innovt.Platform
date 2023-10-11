// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Security.Claims;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters;

/// <summary>
///     This Filter Will inject the username claim in the action Parameters
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class InjectUserNameClaimParameterFilter : ActionFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InjectUserNameClaimParameterFilter"/> class.
    /// </summary>
    /// <param name="defaultAuthorizationProperty">The default authorization property to inject the username.</param>
    /// <param name="actionParameters">The action parameters to inject the username.</param>
    public InjectUserNameClaimParameterFilter(string defaultAuthorizationProperty, params string[] actionParameters)
    {
        Check.NotNull(defaultAuthorizationProperty, nameof(defaultAuthorizationProperty));
        Check.NotNull(actionParameters, nameof(actionParameters));

        DefaultAuthorizationProperty = defaultAuthorizationProperty;
        ActionParameters = actionParameters;
    }

    /// <summary>
    ///     Using defaults: ExternalId  and "filter","command"
    /// </summary>
    public InjectUserNameClaimParameterFilter() : this("ExternalId", "filter", "command")
    {
    }

    /// <summary>
    /// Gets the default authorization property for injecting the username.
    /// </summary>
    public string DefaultAuthorizationProperty { get; }

    /// <summary>
    /// Gets the action parameters to inject the username.
    /// </summary>
    public string[] ActionParameters { get; }

    /// <inheritdoc />
    private void InjectUserName(ActionExecutingContext context)
    {
        if (context is null)
            return;

        var userName = context.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userName.IsNullOrEmpty()) return;

        foreach (var actionParameter in ActionParameters)
        {
            context.ActionArguments.TryGetValue(actionParameter, out var inputParam);

            if (inputParam == null) continue;

            var property = inputParam.GetType().GetProperty(DefaultAuthorizationProperty);

            if (property != null) property.SetValue(inputParam, userName);
        }
    }

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        InjectUserName(context);

        base.OnActionExecuting(context);
    }

    /// <inheritdoc />
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        InjectUserName(context);

        //TODO: Not sure yet
        await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
    }
}