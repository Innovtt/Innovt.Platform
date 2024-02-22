// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.ComponentModel;
using System.Security.Claims;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters;

/// <summary>
///     This Filter Will inject the username claim in the action Parameters
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class InjectClaimParameterFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InjectClaimParameterFilterAttribute" /> class.
    /// </summary>
    /// <param name="defaultAuthorizationProperty">The default authorization property to inject the username.</param>
    /// <param name="actionParameters">The action parameters to inject the username.</param>
    public InjectClaimParameterFilterAttribute(string defaultAuthorizationProperty, params string[] actionParameters)
    {
        Check.NotNull(defaultAuthorizationProperty, nameof(defaultAuthorizationProperty));
        Check.NotNull(actionParameters, nameof(actionParameters));

        DefaultAuthorizationProperty = defaultAuthorizationProperty;
        ActionParameters = actionParameters;
        ClaimTypeCheck = ClaimTypes.NameIdentifier;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InjectClaimParameterFilterAttribute" /> class.
    /// </summary>
    /// <param name="defaultAuthorizationProperty">The default authorization property to inject the username.</param>
    /// <param name="claimTypeCheck"> The claim type that will be used to get the value.</param>
    /// <param name="actionParameters">The action parameters to inject the username.</param>
    public InjectClaimParameterFilterAttribute(string defaultAuthorizationProperty, string claimTypeCheck,
        params string[] actionParameters)
    {
        Check.NotNull(defaultAuthorizationProperty, nameof(defaultAuthorizationProperty));
        Check.NotNull(actionParameters, nameof(actionParameters));
        Check.NotNull(claimTypeCheck, nameof(claimTypeCheck));

        DefaultAuthorizationProperty = defaultAuthorizationProperty;
        ActionParameters = actionParameters;
        ClaimTypeCheck = claimTypeCheck;
    }

    /// <summary>
    ///     Using defaults: ExternalId  and "filter","command"
    /// </summary>
    public InjectClaimParameterFilterAttribute() : this("ExternalId", ClaimTypes.NameIdentifier, "filter", "command")
    {
    }

    /// <summary>
    ///     Gets the default authorization property for injecting the username.
    /// </summary>
    public string DefaultAuthorizationProperty { get; }

    /// <summary>
    ///     Get the default claim type to check.
    /// </summary>
    public string ClaimTypeCheck { get; }

    /// <summary>
    ///     Gets the action parameters to inject the username.
    /// </summary>
    public string[] ActionParameters { get; }

    /// <summary>
    ///     This method will inject the claim in the action parameter. In case of string it will inject the claim value, in
    ///     case of other type it will convert the claim value to the type.
    /// </summary>
    /// <param name="context"></param>
    private void InjectUserName(ActionExecutingContext? context)
    {
        if (context is null)
            return;

        var claimValue = context.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypeCheck)?.Value;

        if (claimValue is null) return;

        foreach (var actionParameter in ActionParameters)
        {
            context.ActionArguments.TryGetValue(actionParameter, out var inputParam);

            var property = inputParam?.GetType().GetProperty(DefaultAuthorizationProperty);

            if (property is null)
                continue;

            object? value = claimValue;

            if (!(property.GetType() is string))
                value = TypeDescriptor.GetConverter(property.PropertyType)
                    .ConvertFromInvariantString(claimValue);

            property.SetValue(inputParam, value);
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

        await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
    }
}