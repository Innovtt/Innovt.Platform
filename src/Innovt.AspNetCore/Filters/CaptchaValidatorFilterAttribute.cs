// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Innovt.AspNetCore.Extensions;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Innovt.AspNetCore.Filters
{
    /// <summary>
    ///     Code by Rafael Cruzeiro: https://github.com/rcruzeiro/Core.Framework/tree/master/Core.Framework.reCAPTCHA
    /// </summary>
    public sealed class CaptchaValidatorFilterAttribute : ActionFilterAttribute
    {
        private const string CaptchaUri = "https://www.google.com/recaptcha/api/siteverify";

        /// <summary>
        /// </summary>
        /// <param name="antiForgery"></param>
        /// <param name="hostName"></param>
        /// <param name="secretKey"></param>
        /// <param name="defaultToken">You can use this parameter if you want to mock you request. You can't set empty string.</param>
        public CaptchaValidatorFilterAttribute(string antiForgery, string hostName, string secretKey,
            string defaultToken = "inn0ut#")
        {
            DefaultToken = defaultToken ?? throw new ArgumentNullException(nameof(defaultToken));
            AntiForgery = antiForgery;
            HostName = hostName;
            SecretKey = secretKey;
        }

        public string AntiForgery { get; }
        public string HostName { get; }
        public string SecretKey { get; internal set; }
        public string DefaultToken { get; }

        private void ReadConfig(HttpContext context)
        {
            if (SecretKey.IsNotNullOrEmpty())
                return;

            if (context.RequestServices.GetService(typeof(IConfiguration)) is not IConfiguration configuration)
                throw new ConfigurationException("IConfiguration Service not found.");

            var captchaSection = configuration.GetSection("Recaptcha");

            if (captchaSection == null)
                throw new ConfigurationException(
                    "Recaptcha section not found. The Recaptcha should contain: SiteKey and SecretKey");

            SecretKey = captchaSection["SecretKey"];

            if (SecretKey.IsNullOrEmpty())
                throw new ConfigurationException("Recaptcha SecretKey can not bu null or empty.");
        }

        private async Task<bool> IsValid(string token, HttpContext context)
        {
            if (string.IsNullOrEmpty(token)) throw new ValidationException("Invalid token.");

            if (DefaultToken.IsNotNullOrEmpty() && token == DefaultToken)
                return true;

            ReadConfig(context);


            using var httpClient = new HttpClient();


            var stringAsync = await httpClient
                .GetStringAsync(new Uri($"{CaptchaUri}?secret={SecretKey}&response={token}"))
                .ConfigureAwait(false);

            var serializerSettings = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };


            var captchaResponse = JsonSerializer.Deserialize<dynamic>(stringAsync, serializerSettings);

            if (captchaResponse is null)
                return false;

            if (captchaResponse.Success & AntiForgery && HostName.IsNotNullOrEmpty() &&
                !captchaResponse.Hostname.Equals(HostName))
                throw new ValidationException(
                    "Captcha hostname and request hostname do not match. Please review anti forgery settings.");

            return captchaResponse.Success;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context != null && !context.HttpContext.IsLocal())
            {
                var header =
                    context.HttpContext.Request.Headers.TryGetValue("g-recaptcha-response", out var recaptchaResponse);

                if (!header)
                {
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new ContentResult
                    {
                        Content = "Missing g-recaptcha-response header.",
                        StatusCode = 400
                    };

                    return;
                }

                var isValid = await IsValid(recaptchaResponse, context.HttpContext).ConfigureAwait(false);

                if (!isValid)
                {
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new ContentResult
                    {
                        Content = "Invalid CAPTCHA Token.",
                        StatusCode = 400
                    };

                    return;
                }
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }
    }
}