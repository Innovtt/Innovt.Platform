// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Innovt.Cloud.AWS.Cognito.Dtos;
using Innovt.Cloud.AWS.Cognito.Exceptions;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Http;
using Innovt.Core.Utilities;
using Innovt.Core.Validation;
using AdminUpdateUserAttributesRequest = Innovt.Cloud.AWS.Cognito.Model.AdminUpdateUserAttributesRequest;
using ChangePasswordRequest = Innovt.Cloud.AWS.Cognito.Model.ChangePasswordRequest;
using CodeMismatchException = Amazon.CognitoIdentityProvider.Model.CodeMismatchException;
using ConfirmForgotPasswordRequest = Innovt.Cloud.AWS.Cognito.Model.ConfirmForgotPasswordRequest;
using ConfirmSignUpRequest = Innovt.Cloud.AWS.Cognito.Model.ConfirmSignUpRequest;
using ExpiredCodeException = Amazon.CognitoIdentityProvider.Model.ExpiredCodeException;
using ForgotPasswordRequest = Innovt.Cloud.AWS.Cognito.Model.ForgotPasswordRequest;
using GetUserRequest = Innovt.Cloud.AWS.Cognito.Model.GetUserRequest;
using InvalidPasswordException = Amazon.CognitoIdentityProvider.Model.InvalidPasswordException;
using PasswordResetRequiredException = Amazon.CognitoIdentityProvider.Model.PasswordResetRequiredException;
using ResendConfirmationCodeRequest = Innovt.Cloud.AWS.Cognito.Model.ResendConfirmationCodeRequest;
using RespondToAuthChallengeRequest = Innovt.Cloud.AWS.Cognito.Model.RespondToAuthChallengeRequest;
using SignUpResponse = Innovt.Cloud.AWS.Cognito.Model.SignUpResponse;
using UpdateUserAttributesRequest = Innovt.Cloud.AWS.Cognito.Model.UpdateUserAttributesRequest;
using UserNotConfirmedException = Amazon.CognitoIdentityProvider.Model.UserNotConfirmedException;
using UserNotFoundException = Amazon.CognitoIdentityProvider.Model.UserNotFoundException;

namespace Innovt.Cloud.AWS.Cognito;

/// <summary>
///     Provides functionality for user management and authentication with Amazon Cognito Identity Provider.
/// </summary>
public abstract class CognitoIdentityProvider : AwsBaseService, ICognitoIdentityProvider
{
    private static readonly ActivitySource CognitoIdentityProviderActivitySource =
        new("Innovt.Cloud.AWS.Cognito.CognitoIdentityProvider");

    private readonly string clientId;
    private readonly CultureInfo cultureInfo = CultureInfo.CurrentCulture;
    private readonly Uri domainEndPoint;
    private readonly string userPoolId;

    private AmazonCognitoIdentityProviderClient cognitoIdentityProvider;

    protected CognitoIdentityProvider(ILogger logger, IAwsConfiguration configuration, string clientId,
        string userPoolId, string domainEndPoint, string region) :
        base(logger, configuration, region)
    {
        ArgumentNullException.ThrowIfNull(domainEndPoint);

        this.clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        this.userPoolId = userPoolId ?? throw new ArgumentNullException(nameof(userPoolId));
        this.domainEndPoint = new Uri(domainEndPoint);
    }

    private AmazonCognitoIdentityProviderClient CognitoProvider
    {
        get { return cognitoIdentityProvider ??= CreateService<AmazonCognitoIdentityProviderClient>(); }
    }

    /// <summary>
    ///     Sends a forgot password request for a user.
    /// </summary>
    /// <param name="command">The <see cref="ForgotPasswordRequest" /> containing user information.</param>
    /// <param name="cancellationToken">A cancellation token for async tasks.</param>
    public virtual async Task ForgotPassword([NotNull] ForgotPasswordRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", command.UserName);

        var forgotRequest = new Amazon.CognitoIdentityProvider.Model.ForgotPasswordRequest
        {
            Username = command.UserName.Trim().ToLower(cultureInfo),
            ClientId = clientId,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
            }
        };

        try
        {
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.ForgotPasswordAsync(forgotRequest, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }


    /// <summary>
    ///     Signs in a user with the provided request for OTP process authentication.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A SignInResponse with a valid token or null</returns>
    public virtual async Task<SignInResponse> SignIn(OtpSignInRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        return await SignIn(AuthFlowType.CUSTOM_AUTH, command, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Signs in a user with the provided authentication information.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<SignInResponse> SignIn(SignInRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        var parameters = new Dictionary<string, string> { { "PASSWORD", command.Password } };

        return await SignIn(AuthFlowType.USER_PASSWORD_AUTH, command, parameters, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     Signs out the user associated with the provided access token.
    /// </summary>
    /// <param name="request">A <see cref="SignOutRequest" /> object containing the access token to sign out the user.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="request" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs during the sign-out process.</exception>
    /// <remarks>
    ///     This method allows you to sign out the user associated with the provided access token. Signing out a user
    ///     invalidates the access
    ///     token, preventing further access to protected resources without reauthentication. Ensure that you provide a valid
    ///     access token
    ///     to successfully sign out the user.
    /// </remarks>
    public async Task SignOut(SignOutRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));
        request.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        var signOutRequest = new GlobalSignOutRequest
        {
            AccessToken = request.AccessToken
        };

        try
        {
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.GlobalSignOutAsync(signOutRequest, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Signs up a new user with the provided registration information.
    /// </summary>
    /// <param name="command">An <see cref="ISignUpRequest" /> object containing the user's registration information.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>
    ///     A <see cref="SignUpResponse" /> indicating whether the user was successfully signed up and their UUID (User
    ///     Sub).
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="command" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs during the sign-up process.</exception>
    /// <remarks>
    ///     This method allows you to sign up a new user with the provided registration information, including username,
    ///     password,
    ///     custom attributes, and other optional properties. After successful sign-up, the user may need to confirm their
    ///     registration
    ///     depending on the authentication flow and configuration.
    /// </remarks>
    public virtual async Task<SignUpResponse> SignUp(ISignUpRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", command.UserName);

        var signUpRequest = new SignUpRequest
        {
            ClientId = clientId,
            Username = command.UserName.Trim().ToLower(cultureInfo),
            Password = command.Password,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
            },
            UserAttributes = []
        };

        if (command.CustomAttributes != null)
        {
            foreach (var attribute in command.CustomAttributes)
            {
                signUpRequest.UserAttributes.Add(new AttributeType
                {
                    Name = $"custom:{attribute.Key}",
                    Value = attribute.Value
                });
            }
        }

        var excludedProperties = new[]
            { "password", "username", "ipaddress", "serverpath", "servername", "httpheader", "customattributes" };


        var properties = command.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => !excludedProperties.Contains(p.Name.ToLower(cultureInfo)));

        foreach (var prop in properties)
        {
            var value = prop.GetValue(command);

            if (value == null) continue;

            signUpRequest.UserAttributes.Add(new AttributeType
            {
                Name = prop.Name.ToLower(cultureInfo),
                Value = value.ToString()
            });
        }

        try
        {
            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoProvider.SignUpAsync(signUpRequest, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);

            return new SignUpResponse
                { Confirmed = response.UserConfirmed.GetValueOrDefault(), UUID = response.UserSub };
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Confirms the sign-up of a user with the specified confirmation code.
    /// </summary>
    /// <param name="request">
    ///     A <see cref="ConfirmSignUpRequest" /> containing the necessary information to confirm the
    ///     sign-up.
    /// </param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="request" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs while confirming the sign-up.</exception>
    /// <remarks>
    ///     This method allows you to confirm the sign-up of a user with the specified confirmation code.
    ///     The confirmation code is typically sent to the user's email or phone number during the registration process.
    ///     After successful confirmation, the user becomes active and can log in.
    /// </remarks>
    public virtual async Task ConfirmSignUp(ConfirmSignUpRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        request.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", request.UserName);

        var confirmSignUpRequest = new Amazon.CognitoIdentityProvider.Model.ConfirmSignUpRequest
        {
            ClientId = clientId,
            Username = request.UserName.Trim().ToLower(cultureInfo),
            ConfirmationCode = request.ConfirmationCode,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{request.IpAddress};ServerPath:{request.ServerPath};ServerName:{request.ServerName}"
            }
        };

        try
        {
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.ConfirmSignUpAsync(confirmSignUpRequest, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Resends the confirmation code to a user with the specified username.
    /// </summary>
    /// <param name="command">
    ///     A <see cref="ResendConfirmationCodeRequest" /> containing the necessary information to resend the
    ///     confirmation code.
    /// </param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="command" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs while resending the confirmation code.</exception>
    /// <remarks>
    ///     This method allows you to resend the confirmation code to a user with the specified username.
    ///     The confirmation code is typically used during the user registration process to verify the user's email or phone
    ///     number.
    /// </remarks>
    public virtual async Task ResendConfirmationCode(ResendConfirmationCodeRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", command.UserName);

        var resendCodeRequest = new Amazon.CognitoIdentityProvider.Model.ResendConfirmationCodeRequest
        {
            ClientId = clientId,
            Username = command.UserName.Trim().ToLower(cultureInfo),
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
            }
        };

        try
        {
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.ResendConfirmationCodeAsync(resendCodeRequest, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Changes the password for a user with the specified access token.
    /// </summary>
    /// <param name="command">
    ///     A <see cref="ChangePasswordRequest" /> containing the necessary information to change the
    ///     password.
    /// </param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="command" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs during password change.</exception>
    /// <remarks>
    ///     This method allows you to change the password for a user by providing an access token and the new proposed
    ///     password.
    ///     The previous password is also required for security verification. If successful, the user's password will be
    ///     updated.
    /// </remarks>
    public virtual async Task ChangePassword(ChangePasswordRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        var changeRequest = new Amazon.CognitoIdentityProvider.Model.ChangePasswordRequest
        {
            AccessToken = command.AccessToken,
            ProposedPassword = command.ProposedPassword,
            PreviousPassword = command.PreviousPassword
        };

        try
        {
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.ChangePasswordAsync(changeRequest, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Retrieves user information based on the specified request and response type.
    /// </summary>
    /// <typeparam name="T">The type of response implementing <see cref="IGetUserResponse" /> to return.</typeparam>
    /// <param name="request">A <see cref="GetUserRequest" /> containing the criteria for retrieving user information.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>An instance of the specified response type containing user information.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="request" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs during user retrieval.</exception>
    /// <remarks>
    ///     This method allows you to retrieve user information based on the specified request, such as
    ///     the user's username or other custom fields. You can specify the response type as a generic
    ///     parameter, which should implement the <see cref="IGetUserResponse" /> interface to provide
    ///     a structured representation of user data. The method returns an instance of the specified
    ///     response type populated with user information.
    /// </remarks>
    public virtual async Task<T> GetUser<T>(GetUserRequest request, CancellationToken cancellationToken = default)
        where T : IGetUserResponse
    {
        request.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        var listUserRequest = new ListUsersRequest
        {
            UserPoolId = userPoolId,
            Filter = $"{request.Field}=\"{request.Value}\""
        };

        try
        {
            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoProvider.ListUsersAsync(listUserRequest, cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            var cognitoUser =
                response?.Users.SingleOrDefault(u =>
                    request.ExcludeExternalUser && u.UserStatus != "EXTERNAL_PROVIDER");

            if (cognitoUser == null)
                return default;

            var user = Activator.CreateInstance<T>();

            user.UserName = cognitoUser.Username;
            user.Status = cognitoUser.UserStatus.ToString(cultureInfo);
            user.UserCreateDate = cognitoUser.UserCreateDate.GetValueOrDefault(DateTime.UtcNow);
            user.UserLastModifiedDate = cognitoUser.UserLastModifiedDate.GetValueOrDefault(DateTime.UtcNow);

            ParseUserAttributes(ref user, cognitoUser.Attributes);

            return user;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Get user information based on the specified request and response type.
    /// </summary>
    /// <param name="accessToken">A valid access token.</param>
    /// <param name="cancellationToken">A cancellationToken token</param>
    /// <typeparam name="T">A response of type IGetUserResponse</typeparam>
    /// <returns>Null or a valid user</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public virtual async Task<T> GetUser<T>(string accessToken, CancellationToken cancellationToken = default)
        where T : IGetUserResponse
    {
        ArgumentNullException.ThrowIfNull(accessToken);

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();

        var getUsersRequest = new Amazon.CognitoIdentityProvider.Model.GetUserRequest
        {
            AccessToken = accessToken
        };

        try
        {
            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoProvider.GetUserAsync(getUsersRequest, cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (response == null)
                return default;

            var user = Activator.CreateInstance<T>();
            user.UserName = response.Username;

            ParseUserAttributes(ref user, response.UserAttributes);

            return user;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Responds to an authentication challenge with the appropriate challenge responses.
    /// </summary>
    /// <param name="command">
    ///     A <see cref="RespondToAuthChallengeRequest" /> containing the necessary information for the
    ///     challenge response.
    /// </param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>An <see cref="AuthChallengeResponse" /> containing the authentication result or metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="command" /> fails validation.</exception>
    /// <exception cref="CriticalException">Thrown when an unsupported challenge name is encountered.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs during the challenge response process.</exception>
    /// <remarks>
    ///     This method is used to respond to various authentication challenges such as custom challenges,
    ///     SMS Multi-Factor Authentication (MFA), and new password requirements. It provides the appropriate
    ///     challenge responses based on the challenge type and returns authentication results or metadata.
    /// </remarks>
    public async Task<AuthChallengeResponse> RespondToAuthChallenge(
        RespondToAuthChallengeRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", command.UserName);

        var challengeResponses = new Dictionary<string, string>
        {
            { "USERNAME", command.UserName.Trim().ToLower(cultureInfo) }
        };

        switch (command.ChallengeName)
        {
            case "CUSTOM_CHALLENGE":
                challengeResponses.Add("ANSWER", command.ConfirmationCode.SmartTrim());
                break;
            case "SMS_MFA":
                challengeResponses.Add("SMS_MFA_CODE", command.ConfirmationCode.SmartTrim());
                break;
            case "NEW_PASSWORD_REQUIRED":
                challengeResponses.Add("NEW_PASSWORD", command.Password);
                break;
            default:
                throw new CriticalException(ErrorCode.ChallengeNotAvailable);
        }

        var request = new Amazon.CognitoIdentityProvider.Model.RespondToAuthChallengeRequest
        {
            ClientId = clientId,
            ChallengeName = command.ChallengeName,
            Session = command.Session,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
            },
            ChallengeResponses = challengeResponses
        };

        try
        {
            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.RespondToAuthChallengeAsync(request, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);

            var result = new AuthChallengeResponse();

            if (response.ResponseMetadata != null) result.Metadata = response.ResponseMetadata.Metadata;

            if (response.AuthenticationResult != null)
                result.AuthenticationResult = new SignInResponse
                {
                    IdToken = response.AuthenticationResult.IdToken,
                    AccessToken = response.AuthenticationResult.AccessToken,
                    ExpiresIn = response.AuthenticationResult.ExpiresIn.GetValueOrDefault(),
                    TokenType = response.AuthenticationResult.TokenType,
                    RefreshToken = response.AuthenticationResult.RefreshToken,
                    SignInType = "USER_PASSWORD_AUTH"
                };

            return result;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Confirms a user's forgotten password and sets a new password for the user.
    /// </summary>
    /// <param name="command">
    ///     A <see cref="ConfirmForgotPasswordRequest" /> containing the necessary information for
    ///     confirmation.
    /// </param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>A task representing the asynchronous confirmation process.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> is null.</exception>
    /// <exception cref="BusinessException">Thrown when the <paramref name="command" /> fails validation.</exception>
    /// <exception cref="CatchException">Thrown when an error occurs during the confirmation process.</exception>
    /// <remarks>
    ///     This method is used to confirm a user's forgotten password by providing the user's username, a new password,
    ///     and a confirmation code received by the user. It sets the new password for the user if the confirmation is
    ///     successful.
    /// </remarks>
    public async Task ConfirmForgotPassword(ConfirmForgotPasswordRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", command.UserName);

        try
        {
            var respond = new Amazon.CognitoIdentityProvider.Model.ConfirmForgotPasswordRequest
            {
                ClientId = clientId,
                Username = command.UserName.Trim().ToLower(cultureInfo),
                Password = command.Password,
                ConfirmationCode = command.ConfirmationCode,
                UserContextData = new UserContextDataType
                {
                    EncodedData =
                        $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
                }
            };

            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.ConfirmForgotPasswordAsync(respond, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Performs social sign-in using an authorization code obtained from an external identity provider.
    /// </summary>
    /// <param name="command">The request containing the authorization code and context information.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>
    ///     A <see cref="OAuth2SignInResponse" /> containing user information and tokens for authentication.
    /// </returns>
    /// <exception cref="BusinessException">Thrown when there is an error in the OAuth2 response.</exception>
    /// <exception cref="CriticalException">Thrown when the user is not found in the system.</exception>
    /// <exception cref="Exception">Thrown for other exceptions during the social sign-in process.</exception>
    /// <remarks>
    ///     This method is used to perform social sign-in using an authorization code obtained from an external identity
    ///     provider.
    ///     It constructs an OAuth2 token request, sends it to the identity provider, and handles the response to authenticate
    ///     the user.
    ///     The method also retrieves user information and determines if the user needs to be registered in the system.
    /// </remarks>
    public virtual async Task<OAuth2SignInResponse> SocialSignIn(SocialSignInRequest command,
        CancellationToken cancellationToken)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ConfirmForgotPassword));
        var parameters = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "authorization_code"),
            new("client_id", clientId),
            new("code", command.Code),
            new("redirect_uri", command.RedirectUri)
        };

        try
        {
            var uri = domainEndPoint.AppendResourceUri("oauth2/token");

            using var httpClient = new HttpClient();
            using var content = new FormUrlEncodedContent(parameters);
            var responseMessage =
                await httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
                throw new BusinessException(ErrorCode.OAuthResponseError);

            var responseContent =
                await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var oauthResponse = JsonSerializer.Deserialize<OAuthSignInResponseDto>(responseContent);

            if (oauthResponse == null) throw new BusinessException(ErrorCode.OAuthResponseError);

            var socialUser = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.GetUserAsync(
                    new Amazon.CognitoIdentityProvider.Model.GetUserRequest
                        { AccessToken = oauthResponse.AccessToken },
                    cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

            if (socialUser == null)
                throw new CriticalException(ErrorCode.UserNotFound);

            var socialUserEmail = GetUserAttributeValue(socialUser.UserAttributes, "email");

            var listUsersResponse = await ListUsersAsync(socialUserEmail, cancellationToken).ConfigureAwait(false);

            var hasCognitoUser = listUsersResponse.Users.Exists(u => u.UserStatus != "EXTERNAL_PROVIDER");

            var response = new OAuth2SignInResponse
            {
                NeedRegister = !hasCognitoUser,
                AccessToken = oauthResponse.AccessToken,
                IdToken = oauthResponse.IdToken,
                RefreshToken = oauthResponse.RefreshToken,
                ExpiresIn = oauthResponse.ExpiresIn,
                TokenType = oauthResponse.TokenType,
                SignInType = GetSocialProviderName(socialUser.Username),
                FirstName = GetUserAttributeValue(socialUser.UserAttributes, "name"),
                LastName = GetUserAttributeValue(socialUser.UserAttributes, "family_name"),
                Picture = GetUserAttributeValue(socialUser.UserAttributes, "picture"),
                Email = socialUserEmail
            };

            //Clear the token to avoid the authentication flow if the user needs to register.
            if (response.NeedRegister)
                response.AccessToken = response.IdToken = response.RefreshToken = null;

            return response;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Refreshes an expired access token using a refresh token, allowing the user to remain authenticated.
    /// </summary>
    /// <param name="command">The request containing the refresh token and context information.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>
    ///     A <see cref="RefreshTokenResponse" /> containing the refreshed access token and associated details.
    /// </returns>
    /// <exception cref="BusinessException">Thrown when the refresh token operation is not authorized.</exception>
    /// <exception cref="Exception">Thrown for other exceptions during the refresh token operation.</exception>
    /// <remarks>
    ///     This method is used to refresh an expired access token using a refresh token, allowing the user to
    ///     remain authenticated without the need for re-entering credentials. The method constructs a refresh token
    ///     authentication request, including user context data, and sends it to the Amazon Cognito Identity Provider
    ///     service. The response contains the refreshed access token and related information.
    /// </remarks>
    public virtual async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        var authRequest = new InitiateAuthRequest
        {
            ClientId = clientId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
            },
            AuthParameters = new Dictionary<string, string>
            {
                { "REFRESH_TOKEN", command.RefreshToken }
            }
        };

        try
        {
            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoProvider.InitiateAuthAsync(authRequest, cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (response.AuthenticationResult == null)
                throw new BusinessException(Messages.NotAuthorized);

            return new RefreshTokenResponse
            {
                IdToken = response.AuthenticationResult.IdToken,
                AccessToken = response.AuthenticationResult.AccessToken,
                ExpiresIn = response.AuthenticationResult.ExpiresIn.GetValueOrDefault(),
                RefreshToken = response.AuthenticationResult.RefreshToken,
                TokenType = response.AuthenticationResult.TokenType
            };
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }


    /// <summary>
    ///     Link user and social account. This is used to avoid billing issues and other problems.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="Exception"></exception>
    public async Task<bool> LinkSocialUser(LinkSocialAccountRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();

        //get users with this email
        var users = await ListUsersAsync(command.Email, cancellationToken);
        //Check if the user has a local user, what mean UserStatus!=EXTERNAL Provider
        var localUser = users.Users.SingleOrDefault(u => u.UserStatus != "EXTERNAL_PROVIDER");

        // If the user is not found, we cannot link the social user.
        if (localUser is null)
        {
            Logger.Info("LinkSocialUser: User with email {@Email} not found in the system. UserName: {@UserName}",
                command.Email, command.UserName);
            return false;
        }

        try
        {
            //Check if the user is a federated user PS: Google_1234567890
            var userNameAndProvider = command.UserName.Split('_');

            if (userNameAndProvider.Length < 2)
                return false;

            var providerName = userNameAndProvider[0];
            var providerValue = command.UserName.Replace(providerName, "", StringComparison.InvariantCultureIgnoreCase)
                .TrimStart('_');
            var identities = localUser.Attributes.SingleOrDefault(a => a.Name == "identities");

            if (identities?.Value?.Contains($"\"{providerName}\"", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                Logger.Info("User {Username} already linked to {Provider}", localUser.Username, providerName);
                return false;
            }

            var request = new AdminLinkProviderForUserRequest
            {
                UserPoolId = userPoolId,
                DestinationUser = new ProviderUserIdentifierType
                {
                    ProviderName = "Cognito",
                    ProviderAttributeValue = localUser.Username
                },
                SourceUser = new ProviderUserIdentifierType
                {
                    ProviderName = providerName,
                    ProviderAttributeName = "Cognito_Subject",
                    ProviderAttributeValue = providerValue
                }
            };

            var result = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.AdminLinkProviderForUserAsync(request, cancellationToken
                )).ConfigureAwait(false);


            return result.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     This method will delete the user using the username and the user pool id. It's important to have the admin delete
    ///     user permission.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> DeleteUser(DeleteUserAccountRequest command, CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();

        var deleteRequest = new AdminDeleteUserRequest
        {
            UserPoolId = userPoolId,
            Username = command.UserName
        };
        try
        {
            var result = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.AdminDeleteUserAsync(deleteRequest, cancellationToken
                )).ConfigureAwait(false);

            return result.HttpStatusCode != HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    /// Clear social accounts linked to a user. The limit is 60 accounts and if you have more you should call this method
    /// multiple times.
    /// This method will delete all social accounts linked to a user with the specified email address.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> ClearSocialAccounts(ClearSocialAccountRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();

        var users = await ListUsersAsync(command.Email, cancellationToken);

        //Check if the user has a local user, what mean UserStatus!=EXTERNAL Provider and confirmed.
        var localUser = users.Users.SingleOrDefault(u => u.UserStatus != UserStatusType.EXTERNAL_PROVIDER);

        // If the user is not found, we cannot clear the social accounts.
        if (localUser is null)
        {
            Logger.Info(
                "User with email {@Email} not found or not confirmed in the system. The process will not continue.",
                command.Email);
            return false;
        }

        var federatedUserNames = users.Users.Where(u => u.UserStatus == UserStatusType.EXTERNAL_PROVIDER)
            .Select(u => u.Username).ToList();

        if (federatedUserNames.Count == 0)
        {
            Logger.Info("No social accounts found for user {@Email}", command.Email);
            return false;
        }

        Logger.Info("Found {@Count} social accounts for user {@Email}", federatedUserNames.Count, command.Email);

        foreach (var userName in federatedUserNames)
        {
            Logger.Info("Removing social account {@Username} for user of email {@Email}.",
                localUser.Username, command.Email);
            await DeleteUser(new DeleteUserAccountRequest(userName), cancellationToken);

            Logger.Info("Social account {@UserName} removed successfully.", userName);
        }

        return true;
    }

    public async Task UpdateUserAttributes(AdminUpdateUserAttributesRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();

        var updateRequest = new Amazon.CognitoIdentityProvider.Model.AdminUpdateUserAttributesRequest
        {
            UserPoolId = userPoolId,
            Username = command.UserName,
            UserAttributes = command.Attributes.Select(ua => new AttributeType
            {
                Name = ua.Key,
                Value = ua.Value
            }).ToList()
        };

        try
        {
            var result = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.AdminUpdateUserAttributesAsync(updateRequest, cancellationToken
                )).ConfigureAwait(false);

            if (result.HttpStatusCode != HttpStatusCode.OK)
                throw new InternalException(ErrorCode.InternalServerError);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Updates user attributes.
    /// </summary>
    /// <param name="command">The <see cref="UpdateUserAttributesRequest" /> containing user attributes.</param>
    /// <param name="cancellationToken">A cancellation token for async tasks.</param>
    public virtual async Task UpdateUserAttributes(UpdateUserAttributesRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        var updateUserAttributeRequest = new Amazon.CognitoIdentityProvider.Model.UpdateUserAttributesRequest
        {
            AccessToken = command.AccessToken,
            UserAttributes = []
        };

        foreach (var (key, value) in command.Attributes)
        {
            updateUserAttributeRequest.UserAttributes.Add(new AttributeType
            {
                Name = key,
                Value = value
            });
        }

        try
        {
            await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.UpdateUserAttributesAsync(updateUserAttributeRequest,
                    cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }


    private static void ParseUserAttributes<T>(ref T user, IList<AttributeType> userAttributes)
        where T : IGetUserResponse
    {
        if (user is null) return;
        if (userAttributes is null) return;

        user.CustomAttributes ??= [];

        foreach (var userAttribute in userAttributes.Where(userAttribute => userAttribute.Name != null))
            switch (userAttribute.Name)
            {
                case "name":
                    user.FirstName = userAttribute.Value;
                    break;
                case "family_name":
                    user.LastName = userAttribute.Value;
                    break;
                default:

                    var propInfo = typeof(T).GetProperty(userAttribute.Name,
                        BindingFlags.IgnoreCase | BindingFlags.Instance |
                        BindingFlags.Public);

                    if (propInfo != null)
                    {
                        propInfo.SetValue(user, userAttribute.Value);
                    }
                    else
                    {
                        user.CustomAttributes.Add(
                            userAttribute.Name.Replace("custom:", "", StringComparison.OrdinalIgnoreCase),
                            userAttribute.Value);
                    }

                    break;
            }
    }

    /// <summary>
    ///     Lists Cognito users matching the provided email address.
    /// </summary>
    /// <param name="email">The email address to filter Cognito users by.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>
    ///     A <see cref="ListUsersResponse" /> containing a list of Cognito users matching the provided email address.
    /// </returns>
    /// <remarks>
    ///     This method is used to retrieve a list of Cognito users that match the provided email address.
    ///     It performs a filter operation based on the email address and returns a list of user records.
    /// </remarks>
    private async Task<ListUsersResponse> ListUsersAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var listUsersResponse = await CognitoProvider.ListUsersAsync(new ListUsersRequest
        {
            UserPoolId = userPoolId,
            Filter = $"email=\"{email}\"",
        }, cancellationToken).ConfigureAwait(false);

        return listUsersResponse;
    }

    /// <summary>
    ///     This method will return the provider name based on the username.
    /// </summary>
    /// <param name="userName">The username provided by cobgnito</param>
    /// <returns></returns>
    private static string GetSocialProviderName(string userName)
    {
        if (!userName.Contains('_', StringComparison.InvariantCultureIgnoreCase))
            return string.Empty;

        var provider = userName.Split("_")[0].ToUpper(CultureInfo.InvariantCulture);
        return provider switch
        {
            "FACEBOOK" => "USER_FACE",
            _ => $"USER_{provider}"
        };
    }

    /// <summary>
    ///     Initiates a user sign-in process using the provided authentication flow type and request parameters.
    /// </summary>
    /// <param name="type">The authentication flow type to use for sign-in.</param>
    /// <param name="request">The sign-in request containing user information and context.</param>
    /// <param name="authParameters">Additional authentication parameters, if needed.</param>
    /// <param name="cancellationToken">A cancellation token for cancelling the operation.</param>
    /// <returns>
    ///     A <see cref="SignInResponse" /> containing the result of the sign-in operation,
    ///     including tokens, session information, or challenge details.
    /// </returns>
    /// <exception cref="Exception">Throws various exceptions based on the sign-in outcome.</exception>
    /// <remarks>
    ///     This method initiates a user sign-in process using the specified authentication flow type,
    ///     such as "USER_PASSWORD_AUTH" for username and password authentication. The method constructs
    ///     an authentication request, including user context data, and sends it to the Amazon Cognito Identity
    ///     Provider service to begin the sign-in process. The response may contain tokens for successful
    ///     sign-in or challenge details for multi-factor authentication (MFA) challenges.
    /// </remarks>
    private async Task<SignInResponse> SignIn(AuthFlowType type, SignInRequestBase request,
        Dictionary<string, string> authParameters = null, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        using var activity = CognitoIdentityProviderActivitySource.StartActivity();
        activity?.SetTag("UserName", request.UserName);

        var authRequest = new InitiateAuthRequest
        {
            ClientId = clientId,
            AuthFlow = type,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{request.IpAddress};ServerPath:{request.ServerPath};ServerName:{request.ServerName}"
            },
            AuthParameters = new Dictionary<string, string>()
            {
                { "USERNAME", request.UserName.Trim().ToLower(cultureInfo) }
            }
        };

        if (authParameters != null)
            foreach (var (key, value) in authParameters)
                authRequest.AuthParameters.Add(key, value);

        try
        {
            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoProvider.InitiateAuthAsync(authRequest, cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (response.AuthenticationResult != null)
                return new SignInResponse
                {
                    IdToken = response.AuthenticationResult.IdToken,
                    AccessToken = response.AuthenticationResult.AccessToken,
                    ExpiresIn = response.AuthenticationResult.ExpiresIn.GetValueOrDefault(),
                    TokenType = response.AuthenticationResult.TokenType,
                    RefreshToken = response.AuthenticationResult.RefreshToken,
                    SignInType = "USER_PASSWORD_AUTH"
                };

            return new SignInResponse
            {
                Session = response.Session,
                ChallengeName = response.ChallengeName.Value,
                ChallengeParameters = response.ChallengeParameters
            };
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    /// <summary>
    ///     Retrieves the value of a user attribute from a collection of attributes.
    /// </summary>
    /// <param name="attributes">A collection of user attributes.</param>
    /// <param name="attributeName">The name of the attribute to retrieve.</param>
    /// <returns>
    ///     The value of the user attribute with the specified name, or null if the attribute
    ///     is not found in the collection.
    /// </returns>
    /// <remarks>
    ///     This method searches for a user attribute in the given collection with a name that
    ///     matches the specified attribute name in a case-insensitive manner. If a matching attribute
    ///     is found, its value is returned. If no matching attribute is found, the method returns null.
    /// </remarks>
    private static string GetUserAttributeValue(IEnumerable<AttributeType> attributes, string attributeName)
    {
        var attribute = attributes?.SingleOrDefault(a =>
            a.Name.Equals(attributeName, StringComparison.OrdinalIgnoreCase));

        return attribute?.Value;
    }

    private static Exception CatchException(Exception ex)
    {
        throw ex switch
        {
            UsernameExistsException => new UserNameAlreadyExistsException(),
            NotAuthorizedException => new UserNotAuthorizedException(),
            UserNotFoundException => new Exceptions.UserNotFoundException(),
            UserNotConfirmedException => new Exceptions.UserNotConfirmedException(),
            PasswordResetRequiredException => new Exceptions.PasswordResetRequiredException(),
            CodeMismatchException => new Exceptions.CodeMismatchException(),
            InvalidPasswordException => new Exceptions.InvalidPasswordException(),
            ExpiredCodeException => new Exceptions.ExpiredCodeException(),
            TooManyRequestsException => new InternalException(ErrorCode.TooManyRequests),
            LimitExceededException => new InternalException(ErrorCode.LimitExceeded),
            InvalidParameterException ipEx => ipEx.Message ==
                                              "Cannot reset password for the user as there is no registered / verified email or phone_number"
                ? new Exceptions.UserNotConfirmedException(ipEx)
                : new InternalException(ipEx),
            BusinessException _ => ex,
            _ => new InternalException(ErrorCode.InternalServerError, ex)
        };
    }

    /// <summary>
    ///     Disposes of resources when they are no longer needed.
    /// </summary>
    protected override void DisposeServices()
    {
        cognitoIdentityProvider?.Dispose();
    }
}