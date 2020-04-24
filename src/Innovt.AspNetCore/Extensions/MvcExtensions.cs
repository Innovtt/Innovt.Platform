using Innovt.AspNetCore.Utility.Pagination;
using Innovt.Core.Exceptions;
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Innovt.AspNetCore.Extensions
{
    public static class MvcExtensions
    {
        /// <summary>
        /// Default Cultures are en, en-US, pt-BR
        /// </summary>
        /// <param name="app"></param>
        /// <param name="supportedCultures"></param>
        public static void UseRequestLocalization(this IApplicationBuilder app, List<CultureInfo> supportedCultures = null)
        {
            if (supportedCultures == null)
                supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("en"), new CultureInfo("en-US"), new CultureInfo("pt"), new CultureInfo("pt-BR")
                };

            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures
            });
        }

        public static void AddBearerAuthorization(this IServiceCollection services, IConfiguration configuration, string configSection = "BearerAuthentication")
        {
            var section = configuration.GetSection(configSection);

            if (section == null)
            {
                throw new CriticalException($"The Config Section '{configSection}' not defined.");
            }

            var audienceSection = section.GetSection("Audience");
            var authoritySection = section.GetSection("Authority");

            if (audienceSection == null)
            {
                throw new CriticalException($"The Config Section 'Audience' not defined.");
            }

            if (authoritySection == null)
            {
                throw new CriticalException($"The Config Section 'Authority' not defined.");
            }

            services.AddBearerAuthorization(audienceSection.Value, authoritySection.Value);
        }

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
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true
                    };
                });
        }

        public static IHtmlContent Pager<T>(this IHtmlHelper helper, PaginationBuilder<T> builder) where T : class
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var html = new StringBuilder();

            if (builder.Collection.TotalRecords < builder.Collection.PageSize && builder.Collection.Page <= 1)
                return new HtmlString(html.ToString());

            
            html.Append(builder.BuildHeader());

            if (builder.Collection.HasPrevious())
            {
                html.Append(builder.BuildPrevious());
            }

            if (builder.Collection.PageCount > 1)
            {
                for (int i = 0; i <= builder.Collection.PageCount - 1; i++)
                {
                    var isCurrent = builder.Collection.Page == i;

                    html.Append(builder.BuildItem(i, isCurrent));
                }
            }

            if (builder.Collection.HasNext())
            {
                html.Append(builder.BuildNext());
            }

            html.Append(builder.BuildFooter());

            html.Append(builder.BuildPagerScript());

            return new HtmlString(html.ToString());
        }
        
        public static SelectList ActiveAndInactiveList()
        {
            var statusList = new List<SelectListItem>()
            {
                new SelectListItem() { Value="1", Text="Ativo" },
                new SelectListItem() { Value="0", Text="Inativo" },
            };

            return new SelectList(statusList, "Value", "Text");
        }


        public static SelectList YesAndNoList()
        {
            var statusList = new List<SelectListItem>()
            {
                new SelectListItem() { Value="1", Text="Sim" },
                new SelectListItem() { Value="0", Text="Não" }
            };

            return new SelectList(statusList, "Value", "Text");
        }


        public static string GetClaim(this ClaimsPrincipal user,string type = ClaimTypes.Email)
        {
            if (user == null)
                return string.Empty;

            var value = (from c in user.Claims
                where c.Type == type
                         select c.Value).FirstOrDefault();

            return value;
        }

        public static bool HasFilter(this ActionDescriptor action, Type filter)
        {
            if (action == null || filter == null)
                return false;

            var exist =  action.FilterDescriptors.Any(f=>f.Filter.GetType() == filter);
            
            return exist;
        }

        /// <summary>
        /// Check if the request is local (Code from Web)
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns></returns>
        public static bool IsLocal(this HttpContext context)
        {
            var remoteIp = context.Connection?.RemoteIpAddress;
            var localIp = context.Connection?.LocalIpAddress;

            if (remoteIp == null && localIp == null)
            {
                return true;
            }


            if (remoteIp != null)
            {
                if (localIp != null)
                {
                    return remoteIp.Equals(localIp);
                }
                else
                {
                    return IPAddress.IsLoopback(remoteIp);

                }
            }
           
            return false;
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            if (session == null)
                throw new System.Exception("Session not available yet.");

            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            if (session == null)
                throw new System.Exception("Session not  available yet.");

            var value = session.GetString(key);

            return value == null ? default :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}
