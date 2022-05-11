// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Innovt.AspNetCore.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "BasicAuthentication";

    private readonly IBasicAuthService authService;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IBasicAuthService authService) : base(options, logger, encoder,
        clock)
    {
        this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    private static AuthenticateResult Fail(string reason)
    {
        return AuthenticateResult.Fail(reason);
    }

    private static AuthenticateResult Success(string username)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, username) };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName));

        return AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName));
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Fail("Missing Authorization Header");

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

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