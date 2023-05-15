// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Http;
using Innovt.Core.Utilities;
using Innovt.Core.Validation;
using ChangePasswordRequest = Innovt.Cloud.AWS.Cognito.Model.ChangePasswordRequest;
using ConfirmForgotPasswordRequest = Innovt.Cloud.AWS.Cognito.Model.ConfirmForgotPasswordRequest;
using ConfirmSignUpRequest = Innovt.Cloud.AWS.Cognito.Model.ConfirmSignUpRequest;
using ForgotPasswordRequest = Innovt.Cloud.AWS.Cognito.Model.ForgotPasswordRequest;
using GetUserRequest = Innovt.Cloud.AWS.Cognito.Model.GetUserRequest;
using ResendConfirmationCodeRequest = Innovt.Cloud.AWS.Cognito.Model.ResendConfirmationCodeRequest;
using RespondToAuthChallengeRequest = Innovt.Cloud.AWS.Cognito.Model.RespondToAuthChallengeRequest;
using SignUpResponse = Innovt.Cloud.AWS.Cognito.Model.SignUpResponse;

namespace Innovt.Cloud.AWS.Cognito;

public abstract class CognitoIdentityProvider : AwsBaseService, ICognitoIdentityProvider
{
    protected static readonly ActivitySource CognitoIdentityProviderActivitySource =
        new("Innovt.Cloud.AWS.Cognito.CognitoIdentityProvider");

    private readonly string clientId;
    private readonly CultureInfo cultureInfo = CultureInfo.CurrentCulture;
    private readonly Uri domainEndPoint;
    private readonly string userPoolId;

    private AmazonCognitoIdentityProviderClient cognitoIdentityProvider;

    protected CognitoIdentityProvider(ILogger logger, IAwsConfiguration configuration, string clientId,
        string userPoolId, string domainEndPoint, string region = null) :
        base(logger, configuration, region)
    {
        this.clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        this.userPoolId = userPoolId ?? throw new ArgumentNullException(nameof(userPoolId));
        if (domainEndPoint == null) throw new ArgumentNullException(nameof(domainEndPoint));

        this.domainEndPoint = new Uri(domainEndPoint);
    }

    private AmazonCognitoIdentityProviderClient CognitoProvider
    {
        get { return cognitoIdentityProvider ??= CreateService<AmazonCognitoIdentityProviderClient>(); }
    }

    public virtual async Task ForgotPassword([NotNull] ForgotPasswordRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ForgotPassword));
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

    public virtual async Task UpdateUserAttributes(UpdateUserAttributeRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(UpdateUserAttributes));
        var updateUserAttributeRequest = new UpdateUserAttributesRequest
        {
            AccessToken = command.AccessToken
        };


        foreach (var (key, value) in command.Attributes)
            updateUserAttributeRequest.UserAttributes.Add(new AttributeType
            {
                Name = key,
                Value = value
            });

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

    public virtual async Task<SignInResponse> SignIn(OtpSignInRequest command,
        CancellationToken cancellationToken = default)
    {
        command.EnsureIsValid();

        return await SignIn(AuthFlowType.CUSTOM_AUTH, command, null, cancellationToken).ConfigureAwait(false);
    }


    public virtual async Task<SignInResponse> SignIn(SignInRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        var parameters = new Dictionary<string, string> { { "PASSWORD", command.Password } };

        return await SignIn(AuthFlowType.USER_PASSWORD_AUTH, command, parameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task SignOut(SignOutRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        request.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(SignOut));
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

    public virtual async Task<SignUpResponse> SignUp(ISignUpRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(SignUp));
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
            }
        };

        if (command.CustomAttributes != null)
            foreach (var attribute in command.CustomAttributes)
                signUpRequest.UserAttributes.Add(new AttributeType
                {
                    Name = $"custom:{attribute.Key}",
                    Value = attribute.Value
                });

        var excludedProperties = new[]
            { "password", "username", "ipaddress", "serverpath", "servername", "httpheader", "customattributes" };


        var properties = command.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => !excludedProperties.Contains(p.Name.ToLower(cultureInfo)));

        foreach (var prop in properties)
        {
            var value = prop.GetValue(command);

            if (value != null)
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

            return new SignUpResponse { Confirmed = response.UserConfirmed, UUID = response.UserSub };
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }


    public virtual async Task ConfirmSignUp(ConfirmSignUpRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        request.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ConfirmSignUp));
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

    public virtual async Task ResendConfirmationCode(ResendConfirmationCodeRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ResendConfirmationCode));
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

    public virtual async Task ChangePassword(ChangePasswordRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ChangePassword));
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

    public virtual async Task<T> GetUser<T>(GetUserRequest request,
        CancellationToken cancellationToken = default) where T : IGetUserResponse
    {
        Check.NotNull(request, nameof(request));

        request.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(GetUser));
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

            var cognitoUser = response?.Users.FirstOrDefault(u =>
                request.ExcludeExternalUser && u.UserStatus != "EXTERNAL_PROVIDER");

            if (cognitoUser == null)
                return default;

            var user = Activator.CreateInstance<T>();

            user.UserName = cognitoUser.Username;
            user.Status = cognitoUser.UserStatus.ToString(cultureInfo);
            user.UserCreateDate = cognitoUser.UserCreateDate;
            user.UserLastModifiedDate = cognitoUser.UserLastModifiedDate;

            foreach (var userAttribute in cognitoUser.Attributes)
            {
                if (userAttribute.Name == null)
                    continue;

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
                            user.CustomAttributes ??= new Dictionary<string, string>();
                            user.CustomAttributes.Add(
                                userAttribute.Name.Replace("custom:", "", StringComparison.OrdinalIgnoreCase),
                                userAttribute.Value);
                        }

                        break;
                }
            }

            return user;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    public async Task<AuthChallengeResponse> RespondToAuthChallenge(
        RespondToAuthChallengeRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(RespondToAuthChallenge));
        activity?.SetTag("UserName", command.UserName);

        var challengeResponses = new Dictionary<string, string>
        {
            { "USERNAME", command.UserName.Trim().ToLower(cultureInfo) }
        };

        switch (command.ChallengeName)
        {
            case "CUSTOM_CHALLENGE":
                challengeResponses.Add("ANSWER", command.ConfirmationCode);
                break;
            case "SMS_MFA":
                challengeResponses.Add("SMS_MFA_CODE", command.ConfirmationCode);
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
                    ExpiresIn = response.AuthenticationResult.ExpiresIn,
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

    public async Task ConfirmForgotPassword(ConfirmForgotPasswordRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ConfirmForgotPassword));
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

    public virtual async Task<OAuth2SignInResponse> SocialSignIn(SocialSignInRequest command,
        CancellationToken cancellationToken)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(ConfirmForgotPassword));
        var parameters = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "authorization_code"),
            new("client_id", clientId),
            new("code", command.Code),
            new("redirect_uri", command.RedirectUri)
        };

        OAuth2SignInResponse response = null;

        try
        {
            var uri = domainEndPoint.AppendResourceUri("oauth2/token");

            using var httpClient = new HttpClient();
            using var content = new FormUrlEncodedContent(parameters);
            var responseMessage =
                await httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
                throw new BusinessException(ErrorCode.OAuthResponseError);

            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            response = JsonSerializer.Deserialize<OAuth2SignInResponse>(responseContent);

            if (response == null)
                throw new BusinessException(ErrorCode.OAuthResponseError);

            var socialUser = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.GetUserAsync(
                    new Amazon.CognitoIdentityProvider.Model.GetUserRequest
                        { AccessToken = response.AccessToken },
                    cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

            if (socialUser == null)
                throw new CriticalException(ErrorCode.UserNotFound);

            var socialUserEmail = GetUserAttributeValue(socialUser.UserAttributes, "email");

            var listUsersResponse = await CognitoProvider.ListUsersAsync(new ListUsersRequest
            {
                UserPoolId = userPoolId,
                Filter = $"email=\"{socialUserEmail}\""
            }, cancellationToken).ConfigureAwait(false);

            var hasCognitoUser = listUsersResponse.Users.Any(u => u.UserStatus != "EXTERNAL_PROVIDER");

            response.NeedRegister = !hasCognitoUser;
            response.SignInType =
                "Facebook_".StartsWith(socialUser.Username, StringComparison.InvariantCultureIgnoreCase)
                    ? "USER_FACE"
                    : "USER_GOOGLE";

            response.FirstName = GetUserAttributeValue(socialUser.UserAttributes, "name");
            response.LastName = GetUserAttributeValue(socialUser.UserAttributes, "family_name");
            response.Picture = GetUserAttributeValue(socialUser.UserAttributes, "picture");
            response.Email = socialUserEmail;

            if (response.NeedRegister) response.AccessToken = response.IdToken = response.RefreshToken = null;
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }

        return response;
    }

    public virtual async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest command,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(command, nameof(command));

        command.EnsureIsValid();

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(RefreshToken));
        var authRequest = new InitiateAuthRequest
        {
            ClientId = clientId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
            }
        };

        authRequest.AuthParameters.Add("REFRESH_TOKEN", command.RefreshToken);

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
                ExpiresIn = response.AuthenticationResult.ExpiresIn,
                TokenType = response.AuthenticationResult.TokenType
            };
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }


    private static Exception CatchException(Exception ex)
    {
        throw ex switch
        {
            UsernameExistsException => new BusinessException(ErrorCode.UsernameAlreadyExists, ex),
            NotAuthorizedException => new BusinessException(ErrorCode.NotAuthorized, ex),
            TooManyRequestsException => new BusinessException(ErrorCode.TooManyRequests, ex),
            PasswordResetRequiredException => new BusinessException(ErrorCode.PasswordResetRequired, ex),
            UserNotFoundException => new BusinessException(ErrorCode.UserNotFound, ex),
            UserNotConfirmedException => new BusinessException(ErrorCode.UserNotConfirmed, ex),
            InvalidPasswordException => new BusinessException(ErrorCode.InvalidPassword, ex),
            CodeMismatchException => new BusinessException(ErrorCode.CodeMismatch, ex),
            ExpiredCodeException => new BusinessException(ErrorCode.ExpiredCode, ex),
            LimitExceededException => new BusinessException(ErrorCode.LimitExceeded, ex),
            InvalidParameterException ipEx => ipEx.Message ==
                                              "Cannot reset password for the user as there is no registered / verified email or phone_number"
                ? new BusinessException(ErrorCode.UserNotConfirmed, ex)
                : ipEx,
            BusinessException _ => ex,
            _ => new CriticalException(ErrorCode.InternalServerError, ex)
        };
    }
    private async Task<SignInResponse> SignIn(AuthFlowType type, SignInRequestBase request,
        Dictionary<string, string> authParameters = null, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        using var activity = CognitoIdentityProviderActivitySource.StartActivity(nameof(SignIn));
        activity?.SetTag("UserName", request.UserName);

        var authRequest = new InitiateAuthRequest
        {
            ClientId = clientId,
            AuthFlow = type,
            UserContextData = new UserContextDataType
            {
                EncodedData =
                    $"IP:{request.IpAddress};ServerPath:{request.ServerPath};ServerName:{request.ServerName}"
            }
        };

        authRequest.AuthParameters.Add("USERNAME", request.UserName.Trim().ToLower(cultureInfo));

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
                    ExpiresIn = response.AuthenticationResult.ExpiresIn,
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
        catch (NotAuthorizedException)
        {
            throw new BusinessException(ErrorCode.NotAuthorized);
        }
        catch (Exception ex)
        {
            throw CatchException(ex);
        }
    }

    private static string GetUserAttributeValue(IEnumerable<AttributeType> attributes, string attributeName)
    {
        var attribute = attributes?.SingleOrDefault(a =>
            a.Name.Equals(attributeName, StringComparison.OrdinalIgnoreCase));

        return attribute?.Value;
    }

    protected override void DisposeServices()
    {
        cognitoIdentityProvider?.Dispose();
    }
}