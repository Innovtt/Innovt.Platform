// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Innovt.AspNetCore.Utility.Pagination;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Innovt.AspNetCore.Extensions;

/// <summary>
///     Extension methods for configuring MVC-related functionality.
/// </summary>
public static class MvcExtensions
{
    /// <summary>
    ///     Default Cultures are en, en-US, pt-BR
    /// </summary>
    /// <param name="app"></param>
    /// <param name="supportedCultures"></param>
    public static void UseRequestLocalization(this IApplicationBuilder app,
        IList<CultureInfo> supportedCultures = null!)
    {
        supportedCultures ??= new List<CultureInfo>
        {
            new("en"), new("en-US"), new("pt"), new("pt-BR")
        };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = supportedCultures
        });
    }

    /// <summary>
    ///     Adds the application scope to the request headers.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="scope">The application scope.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseApplicationScope(this IApplicationBuilder app, string scope)
    {
        if (scope.IsNullOrEmpty())
            return app;

        return app.Use(async (context, next) =>
        {
            context.Request.Headers.Add(Constants.HeaderApplicationScope, scope);
            await next().ConfigureAwait(false);
        });
    }

    /// <summary>
    ///     Sets the application context header in the request headers.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="headerContext">The header context value.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder SetHeaderApplicationContext(this IApplicationBuilder app, string headerContext)
    {
        if (headerContext.IsNullOrEmpty())
            return app;

        return app.Use(async (context, next) =>
        {
            context.Request.Headers.Add(Constants.HeaderApplicationContext, headerContext);
            await next().ConfigureAwait(false);
        });
    }

    /// <summary>
    ///     Adds Bearer token authentication based on the provided configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="configSection">The configuration section name.</param>
    /// <param name="validateAudience">Whether to validate audience.</param>
    /// <param name="validateIssuer">Whether to validate issuer.</param>
    /// <param name="validateLifetime">Whether to validate lifetime.</param>
    /// <param name="validateIssuerSigningKey">Whether to validate issuer signing key.</param>
    public static void AddBearerAuthorization(this IServiceCollection services, IConfiguration configuration,
        string configSection = "BearerAuthentication", bool validateAudience = true,
        bool validateIssuer = true, bool validateLifetime = true, bool validateIssuerSigningKey = true)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var audienceSection = configuration.GetSection($"{configSection}:Audience");
        var authoritySection = configuration.GetSection($"{configSection}:Authority");

        if (audienceSection.Value == null)
            throw new CriticalException($"The Config Section '{configSection}:Audience' not defined.");
        if (authoritySection.Value == null)
            throw new CriticalException("The Config Section '{configSection}:Authority' not defined.");

        services.AddBearerAuthorization(audienceSection.Value, authoritySection.Value, validateAudience, validateIssuer,
            validateLifetime, validateIssuerSigningKey);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    ///     Adds Bearer token authentication.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="audienceId">The audience ID.</param>
    /// <param name="authority">The authority.</param>
    /// <param name="validateAudience">Whether to validate audience.</param>
    /// <param name="validateIssuer">Whether to validate issuer.</param>
    /// <param name="validateLifetime">Whether to validate lifetime.</param>
    /// <param name="validateIssuerSigningKey">Whether to validate issuer signing key.</param>
    public static void AddBearerAuthorization(this IServiceCollection services, string audienceId, string authority,
        bool validateAudience = true,
        bool validateIssuer = true, bool validateLifetime = true, bool validateIssuerSigningKey = true)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build();
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = audienceId;
                options.Authority = authority;
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = validateIssuerSigningKey,
                    ValidateAudience = validateAudience,
                    ValidateIssuer = validateIssuer,
                    ValidateLifetime = validateLifetime
                };
            });
    }

    /// <summary>
    ///     Generates an HTML pager for pagination.
    /// </summary>
    /// <typeparam name="T">The type of items being paginated.</typeparam>
    /// <param name="helper">The HTML helper.</param>
    /// <param name="builder">The pagination builder.</param>
    /// <returns>The HTML pager content.</returns>
    public static IHtmlContent Pager<T>(this IHtmlHelper helper, PaginationBuilder<T> builder) where T : class
    {
        if (helper is null) throw new ArgumentNullException(nameof(helper));


        if (builder == null) throw new ArgumentNullException(nameof(builder));

        if (builder.Collection.TotalRecords < builder.Collection.PageSize &&
            builder.Collection.IsNumberPagination && builder.Collection.Page != null && int.Parse(
                builder.Collection.Page,
                CultureInfo.InvariantCulture) <= 1)
            return new HtmlString(string.Empty);

        var html = new StringBuilder();

        html.Append(builder.BuildHeader());

        if (builder.Collection.HasPrevious()) html.Append(builder.BuildPrevious());

        if (builder.Collection.PageCount > 1)
            for (var i = 0; i <= builder.Collection.PageCount - 1; i++)
            {
                var isCurrent = builder.Collection.Page == i.ToString(CultureInfo.InvariantCulture);

                html.Append(builder.BuildItem(i, isCurrent));
            }

        if (builder.Collection.HasNext()) html.Append(builder.BuildNext());

        html.Append(builder.BuildFooter());

        html.Append(builder.BuildPagerScript());

        return new HtmlString(html.ToString());
    }

    /// <summary>
    ///     Creates a select list containing "Ativo" and "Inativo" items.
    /// </summary>
    /// <returns>The select list.</returns>
    public static SelectList ActiveAndInactiveList()
    {
        var statusList = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Ativo" },
            new() { Value = "0", Text = "Inativo" }
        };

        return new SelectList(statusList, "Value", "Text");
    }

    /// <summary>
    ///     Creates a select list containing "Sim" and "Não" items.
    /// </summary>
    /// <returns>The select list.</returns>
    public static SelectList YesAndNoList()
    {
        var statusList = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Sim" },
            new() { Value = "0", Text = "Não" }
        };

        return new SelectList(statusList, "Value", "Text");
    }

    /// <summary>
    ///     Gets the value of a claim from the user's claims principal.
    /// </summary>
    /// <param name="user">The claims principal.</param>
    /// <param name="type">The claim type (default is ClaimTypes.Email).</param>
    /// <returns>The claim value or an empty string if not found.</returns>
    public static string GetClaim(this ClaimsPrincipal user, string type = ClaimTypes.Email)
    {
        if (user is null)
            return string.Empty;

        var value = (from c in user.Claims
            where c.Type == type
            select c.Value).FirstOrDefault();

        return value ?? string.Empty;
    }

    /// <summary>
    ///     Checks if the specified action descriptor has a filter of the given type.
    /// </summary>
    /// <param name="action">The action descriptor.</param>
    /// <param name="filter">The type of filter to check for.</param>
    /// <returns>True if the action has the filter, otherwise false.</returns>
    public static bool HasFilter(this ActionDescriptor action, Type filter)
    {
        if (action == null || filter == null)
            return false;

        var exist = action.FilterDescriptors.Any(f => f.Filter.GetType() == filter);

        return exist;
    }

    /// <summary>
    ///     Check if the request is local (Code from Web)
    /// </summary>
    /// <param name="context">The current context</param>
    /// <returns></returns>
    public static bool IsLocal(this HttpContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var remoteIp = context.Connection?.RemoteIpAddress;
        var localIp = context.Connection?.LocalIpAddress;

        if (remoteIp == null && localIp == null) return true;


        if (remoteIp != null)
        {
            if (localIp != null)
                return remoteIp.Equals(localIp);
            return IPAddress.IsLoopback(remoteIp);
        }

        return false;
    }

    /// <summary>
    ///     Sets an object in the session after serializing it to JSON.
    /// </summary>
    /// <typeparam name="T">The type of the object to be stored.</typeparam>
    /// <param name="session">The session object.</param>
    /// <param name="key">The key to store the object under.</param>
    /// <param name="value">The object to be stored.</param>
    public static void Set<T>(this ISession session, string key, T value)
    {
        session?.SetString(key, JsonSerializer.Serialize(value));
    }

    /// <summary>
    ///     Gets an object from the session and deserializes it from JSON.
    /// </summary>
    /// <typeparam name="T">The type of the object to be retrieved.</typeparam>
    /// <param name="session">The session object.</param>
    /// <param name="key">The key the object was stored under.</param>
    /// <returns>The deserialized object.</returns>
    public static T Get<T>(this ISession session, string key)
    {
        var value = session?.GetString(key);

        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}