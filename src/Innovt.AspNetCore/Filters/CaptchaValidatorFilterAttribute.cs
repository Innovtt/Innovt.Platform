// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Text.Json;
using System.Text.Json.Serialization;
using Innovt.AspNetCore.Model;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Innovt.AspNetCore.Filters;

/// <summary>
///     Represents the response received from reCAPTCHA verification.
/// </summary>
internal class RecaptchaResponse
{
    /// <summary>
    ///     Indicates whether the reCAPTCHA verification was successful.
    /// </summary>
    public bool success { get; set; }

    /// <summary>
    ///     The score obtained from the reCAPTCHA verification.
    /// </summary>
    public decimal score { get; set; }

    /// <summary>
    ///     The action associated with the reCAPTCHA verification.
    /// </summary>
    public string action { get; set; }

    /// <summary>
    ///     The timestamp of the challenge.
    /// </summary>
    public string challenge_ts { get; set; }

    /// <summary>
    ///     The hostname from which the reCAPTCHA verification originated.
    /// </summary>
    public string hostname { get; set; }
}

/// <summary>
///     Code by Rafael Cruzeiro: https://github.com/rcruzeiro/Core.Framework/tree/master/Core.Framework.reCAPTCHA
/// </summary>
/// <summary>
///     Action filter attribute for validating reCAPTCHA tokens.
/// </summary>
public sealed class CaptchaValidatorFilterAttribute : ActionFilterAttribute
{
    private const string CaptchaUri = "https://www.google.com/recaptcha/api/siteverify";

    /// <summary>
    ///     Initializes a new instance of the <see cref="CaptchaValidatorFilterAttribute" /> class.
    /// </summary>
    public CaptchaValidatorFilterAttribute()
    {
        DefaultToken = "inn0ut#";
    }

    /// <summary>
    ///     Gets or sets the anti-forgery token.
    /// </summary>
    public string AntiForgery { get; }

    /// <summary>
    ///     Gets or sets the hostname.
    /// </summary>
    public string HostName { get; }

    /// <summary>
    ///     Gets or sets the secret key for reCAPTCHA validation.
    /// </summary>
    public string SecretKey { get; internal set; }

    /// <summary>
    ///     Gets or sets the default reCAPTCHA token to accept without validation.
    /// </summary>
    public string DefaultToken { get; set; }

    /// <summary>
    ///     Reads the reCAPTCHA configuration from the configuration provider.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
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

    /// <summary>
    ///     Validates the reCAPTCHA token using the Google reCAPTCHA API.
    /// </summary>
    /// <param name="token">The reCAPTCHA token.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The task result contains a boolean indicating
    ///     whether the reCAPTCHA is valid.
    /// </returns>
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
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var captchaResponse = JsonSerializer.Deserialize<RecaptchaResponse>(stringAsync, serializerSettings);

        if (captchaResponse is null)
            return false;

        return captchaResponse.success;
    }

    /// <summary>
    ///     Overrides the <see cref="ActionFilterAttribute.OnActionExecutionAsync" /> method to validate the reCAPTCHA token.
    /// </summary>
    /// <param name="context">The action executing context.</param>
    /// <param name="next">The action execution delegate.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
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