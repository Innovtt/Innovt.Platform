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

public static class MvcExtensions
{
    /// <summary>
    ///     Default Cultures are en, en-US, pt-BR
    /// </summary>
    /// <param name="app"></param>
    /// <param name="supportedCultures"></param>
    public static void UseRequestLocalization(this IApplicationBuilder app, IList<CultureInfo> supportedCultures = null!)
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

    public static void AddBearerAuthorization(this IServiceCollection services, IConfiguration configuration, string configSection = "BearerAuthentication")
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var audienceSection = configuration.GetSection($"{configSection}:Audience");
        var authoritySection = configuration.GetSection($"{configSection}:Authority");
        
        if (audienceSection.Value == null) throw new CriticalException($"The Config Section '{configSection}:Audience' not defined.");
        if (authoritySection.Value == null) throw new CriticalException("The Config Section '{configSection}:Authority' not defined.");

        services.AddBearerAuthorization(audienceSection.Value, authoritySection.Value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static void AddBearerAuthorization(this IServiceCollection services, string audienceId, string authority)
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
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                };
            });
    }

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

    public static SelectList ActiveAndInactiveList()
    {
        var statusList = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Ativo" },
            new() { Value = "0", Text = "Inativo" }
        };

        return new SelectList(statusList, "Value", "Text");
    }


    public static SelectList YesAndNoList()
    {
        var statusList = new List<SelectListItem>
        {
            new() { Value = "1", Text = "Sim" },
            new() { Value = "0", Text = "Não" }
        };

        return new SelectList(statusList, "Value", "Text");
    }


    public static string GetClaim(this ClaimsPrincipal user, string type = ClaimTypes.Email)
    {
        if (user is null)
            return string.Empty;

        var value = (from c in user.Claims
            where c.Type == type
            select c.Value).FirstOrDefault();

        return value ?? string.Empty;
    }

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

    public static void Set<T>(this ISession session, string key, T value)
    {
        session?.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session?.GetString(key);

        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}