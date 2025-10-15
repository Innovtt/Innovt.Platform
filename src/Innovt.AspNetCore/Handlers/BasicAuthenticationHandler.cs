// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Innovt.Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Innovt.AspNetCore.Handlers;

/// <summary>
///     Custom authentication handler for handling basic authentication in ASP.NET Core.
/// </summary>
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    ///     The scheme name for basic authentication.
    /// </summary>
    public const string SchemeName = "BasicAuthentication";

    private readonly IBasicAuthService authService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BasicAuthenticationHandler" /> class.
    /// </summary>
    /// <param name="options">The options for the authentication scheme.</param>
    /// <param name="logger">The logger factory.</param>
    /// <param name="encoder">The URL encoder.</param>
    /// <param name="authService">The custom basic authentication service.</param>
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, IBasicAuthService authService) : base(options, logger, encoder)
    {
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    /// <summary>
    ///     Generates an authentication failure response.
    /// </summary>
    /// <param name="reason">The reason for the failure.</param>
    /// <returns>An authentication result indicating failure.</returns>
    private static AuthenticateResult Fail(string reason)
    {
        return AuthenticateResult.Fail(reason);
    }

    /// <summary>
    ///     Generates an authentication success response.
    /// </summary>
    /// <param name="username">The authenticated username.</param>
    /// <returns>An authentication result indicating success.</returns>
    private static AuthenticateResult Success(string username)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, username) };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName));

        return AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName));
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var value))
            return Fail("Missing Authorization Header");

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(value!);

            if (authHeader?.Parameter is null)
                return Fail("Invalid Authorization Header");

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            var username = credentials[0];
            var password = credentials[1];

            var authenticated = await authService.Authenticate(username, password).ConfigureAwait(false);

            if (authenticated)
                return Success(username);
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            return Fail($"Authentication failure: {ex.Message}");
        }

        return Fail("Invalid user or password");
    }
}