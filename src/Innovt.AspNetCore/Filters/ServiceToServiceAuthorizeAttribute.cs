using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters;

/// <summary>
/// This filter is used to authorize service-to-service communication by checking a specific header.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ServiceToServiceAuthorizeAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderName = "X-Internal-Key";
    public string ExpectedKey { get; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ServiceToServiceAuthorizeAttribute(string expectedKey)
    {
        ExpectedKey = expectedKey ?? throw new ArgumentNullException(nameof(expectedKey));
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(context);

        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var providedKey) ||
            !string.Equals(providedKey, ExpectedKey, StringComparison.Ordinal))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}