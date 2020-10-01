using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
    /// Code by Rafael Cruzeiro: https://github.com/rcruzeiro/Core.Framework/tree/master/Core.Framework.reCAPTCHA
    /// </summary>
    public class CaptchaValidatorFilterAttribute : ActionFilterAttribute
    {
        private readonly string antiForgery;
        private readonly string hostName;
        private static string secretKey;
        private const string captchaURI = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string defaultToken = "inn0ut#";

         /// <summary>
         /// 
         /// </summary>
         /// <param name="antiForgery"></param>
         /// <param name="hostName"></param>
         /// <param name="defaultToken">You can use this parameter if you want to mock you request. You can't set empty string.</param>
        public CaptchaValidatorFilterAttribute(string antiForgery=null, string hostName=null,string defaultToken = "inn0ut#")
        {
            this.antiForgery = antiForgery;
            this.hostName = hostName;
            this.defaultToken = defaultToken ?? throw new ArgumentNullException(nameof(defaultToken));
        }

        private void ReadConfig(HttpContext context)
        {
            if(secretKey.IsNotNullOrEmpty())
                return;


            if (!(context.RequestServices.GetService(typeof(IConfiguration)) is IConfiguration configuration))
                throw new ConfigurationException("IConfiguration Service not found.");

            var captchaSection = configuration.GetSection("Recaptcha");

            if (captchaSection == null)
                throw new ConfigurationException("Recaptcha section not found. The Recaptcha should contain: SiteKey and SecretKey");

            secretKey = captchaSection["SecretKey"];

            if(secretKey.IsNullOrEmpty())
                throw new ConfigurationException("Recaptcha SecretKey can not bu null or empty.");
        }

        private async Task<bool> IsValid(string token, HttpContext context)
        {
            if (string.IsNullOrEmpty(token)) throw new ValidationException("Invalid token.");

            if (defaultToken.IsNotNullOrEmpty() && token == defaultToken)
                return true;

            ReadConfig(context);


            using var httpClient = new HttpClient();
            
                var stringAsync = await httpClient.GetStringAsync($"{captchaURI}?secret={secretKey}&response={token}");

                var serializerSettings = new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                };
            

            dynamic captchaResponse = JsonSerializer.Deserialize<dynamic>(stringAsync, serializerSettings);

            if (captchaResponse.Success & antiForgery && hostName.IsNotNullOrEmpty() &&
                !captchaResponse.Hostname.Equals(hostName))
                throw new ValidationException(
                    "Captcha hostname and request hostname do not match. Please review anti forgery settings.");

            return captchaResponse.Success;
        
    }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {  
            if (!context.HttpContext.IsLocal())
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

                var isValid = await this.IsValid(recaptchaResponse,context.HttpContext);

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

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
