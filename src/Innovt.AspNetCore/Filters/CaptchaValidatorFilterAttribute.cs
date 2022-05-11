// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.AspNetCore.Model;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Innovt.AspNetCore.Filters;

internal class RecaptchaResponse
{
    public bool success { get; set; }
    public decimal score { get; set; }
    public string action { get; set; }
    public string challenge_ts { get; set; }
    public string hostname { get; set; }
}

/// <summary>
///     Code by Rafael Cruzeiro: https://github.com/rcruzeiro/Core.Framework/tree/master/Core.Framework.reCAPTCHA
/// </summary>
public sealed class CaptchaValidatorFilterAttribute : ActionFilterAttribute
{
    private const string CaptchaUri = "https://www.google.com/recaptcha/api/siteverify";
    public string AntiForgery { get; }
    public string HostName { get; }
    public string SecretKey { get; internal set; }
    public string DefaultToken { get; set; }

    public CaptchaValidatorFilterAttribute()
    {
        DefaultToken = "inn0ut#";
    }

    private void ReadConfig(HttpContext context)
    {
        if (SecretKey.IsNotNullOrEmpty())
            return;

        if (context.RequestServices.GetService(typeof(IConfiguration)) is not IConfiguration configuration)
            throw new ConfigurationException("IConfiguration Service not found.");

        SecretKey = configuration.GetSection("Recaptcha")["SecretKey"];

        if (SecretKey.IsNullOrEmpty())
            throw new ConfigurationException(
                "Recaptcha section not found.The Recaptcha should contain: Recaptcha:SecretKey");
    }

    private async Task<bool> IsValid(string token, HttpContext context)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        if (token == DefaultToken)
            return true;

        ReadConfig(context);

        using var httpClient = new HttpClient();

        var stringAsync = await httpClient
            .GetStringAsync(new Uri($"{CaptchaUri}?secret={SecretKey}&response={token}"))
            .ConfigureAwait(false);

        var serializerSettings = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var captchaResponse = JsonSerializer.Deserialize<RecaptchaResponse>(stringAsync, serializerSettings);

        if (captchaResponse is null)
            return false;

        return captchaResponse.success;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context is null) return;

        var header = context.HttpContext.Request.Headers.TryGetValue("g-recaptcha-response", out var recaptchaResponse);

        var result = new ResponseError
        {
            Code = $"{StatusCodes.Status400BadRequest}",
            TraceId = context.HttpContext.TraceIdentifier
        };

        if (!header)
        {
            result.Message = "Missing g-recaptcha-response header.";
            context.Result = new BadRequestObjectResult(result);

            return;
        }

        var isValid = await IsValid(recaptchaResponse, context.HttpContext).ConfigureAwait(false);

        if (!isValid)
        {
            result.Message = "Invalid CAPTCHA Token";
            context.Result = new BadRequestObjectResult(result);
            return;
        }


        await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
    }
}