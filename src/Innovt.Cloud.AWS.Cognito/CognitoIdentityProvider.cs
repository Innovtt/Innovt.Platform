using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Http;
using Innovt.Core.Utilities;
using Innovt.Core.Validation;

namespace Innovt.Cloud.AWS.Cognito
{
    public abstract class CognitoIdentityProvider : AwsBaseService, ICognitoIdentityProvider
    {
        private readonly string clientId;
        private readonly string userPoolId;
        private readonly Uri domainEndPoint;

        protected CognitoIdentityProvider(ILogger logger, IAWSConfiguration configuration, string clientId,
            string userPoolId, string domainEndPoint, string region = null) :
            base(logger, configuration, region)
        {
            this.clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            this.userPoolId = userPoolId ?? throw new ArgumentNullException(nameof(userPoolId));
            if (domainEndPoint == null) throw new ArgumentNullException(nameof(domainEndPoint));

            this.domainEndPoint = new Uri(domainEndPoint);
        }

        private AmazonCognitoIdentityProviderClient cognitoidentityProvider;

        private AmazonCognitoIdentityProviderClient CognitoidentityProvider =>
            cognitoidentityProvider ??= CreateService<AmazonCognitoIdentityProviderClient>();


        private Exception CatchException(Exception ex)
        {
            throw ex switch
            {
                UsernameExistsException _ => new BusinessException(ErrorCode.UsernameAlreadyExists, ex),
                NotAuthorizedException _ => new BusinessException(ErrorCode.NotAuthorized, ex),
                TooManyRequestsException _ => new BusinessException(ErrorCode.TooManyRequests, ex),
                PasswordResetRequiredException _ => new BusinessException(ErrorCode.PasswordResetRequired, ex),
                UserNotFoundException _ => new BusinessException(ErrorCode.UserNotFound, ex),
                UserNotConfirmedException _ => new BusinessException(ErrorCode.UserNotConfirmed, ex),
                InvalidPasswordException _ => new BusinessException(ErrorCode.InvalidPassword, ex),
                CodeMismatchException _ => new BusinessException(ErrorCode.CodeMismatch, ex),
                ExpiredCodeException _ => new BusinessException(ErrorCode.ExpiredCode, ex),
                LimitExceededException _ => new BusinessException(ErrorCode.LimitExceeded, ex),
                BusinessException _ => ex,
                _ => new CriticalException(ErrorCode.InternalServerError, ex)
            };
        }

        public virtual async Task ForgotPassword(Model.ForgotPasswordRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var forgotRequest = new Amazon.CognitoIdentityProvider.Model.ForgotPasswordRequest
            {
                Username = command.UserName.ToLower(),
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
                    await CognitoidentityProvider.ForgotPasswordAsync(forgotRequest, cancellationToken));
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

            var updateUserAttributeRequest = new UpdateUserAttributesRequest
            {
                AccessToken = command.AccessToken
            };


            foreach (var attr in command.Attributes)
                updateUserAttributeRequest.UserAttributes.Add(new AttributeType()
                {
                    Name = attr.Key,
                    Value = attr.Value
                });

            try
            {
                await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.UpdateUserAttributesAsync(updateUserAttributeRequest,
                        cancellationToken));
            }
            catch (Exception ex)
            {
                throw CatchException(ex);
            }
        }


        private async Task<SignInResponse> SignIn(AuthFlowType type, SignInRequestBase request,
            Dictionary<string, string> authParameters = null, CancellationToken cancellationToken = default)
        {
            Check.NotNull(request, nameof(request));

            var authRequest = new InitiateAuthRequest()
            {
                ClientId = clientId,
                AuthFlow = type,
                UserContextData = new UserContextDataType
                {
                    EncodedData =
                        $"IP:{request.IpAddress};ServerPath:{request.ServerPath};ServerName:{request.ServerName}"
                }
            };

            authRequest.AuthParameters.Add("USERNAME", request.UserName.ToLower());

            if (authParameters != null)
                foreach (var (key, value) in authParameters)
                    authRequest.AuthParameters.Add(key, value);

            try
            {
                var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.InitiateAuthAsync(authRequest, cancellationToken));

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

        public virtual async Task<SignInResponse> SignIn(OtpSignInRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            return await SignIn(AuthFlowType.CUSTOM_AUTH, command, null, cancellationToken);
        }


        public virtual async Task<SignInResponse> SignIn(SignInRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var parameters = new Dictionary<string, string>() {{"PASSWORD", command.Password}};

            return await SignIn(AuthFlowType.USER_PASSWORD_AUTH, command, parameters, cancellationToken);
        }

        public async Task SignOut(SignOutRequest request, CancellationToken cancellationToken = default)
        {
            Check.NotNull(request, nameof(request));

            request.EnsureIsValid();

            var signOutRequest = new GlobalSignOutRequest
            {
                AccessToken = request.AccessToken
            };

            try
            {
                await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.GlobalSignOutAsync(signOutRequest, cancellationToken));
            }
            catch (Exception ex)
            {
                throw CatchException(ex);
            }
        }


        private string GetUserAttributeValue(List<AttributeType> attributes, string attributeName)
        {
            var attribute = attributes.SingleOrDefault(a =>
                a.Name.Equals(attributeName, StringComparison.CurrentCultureIgnoreCase));

            return attribute?.Value;
        }

        public virtual async Task<Model.SignUpResponse> SignUp(ISignUpRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var signUpRequest = new SignUpRequest
            {
                ClientId = clientId,
                Username = command.UserName.ToLower(),
                Password = command.Password,
                UserContextData = new UserContextDataType
                {
                    EncodedData =
                        $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
                }
            };

            var excludedProperties = new[]
                {"password", "username", "ipaddress", "serverpath", "servername", "httpheader"};

            var properties = command.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !excludedProperties.Contains(p.Name.ToLower()));

            foreach (var prop in properties)
            {
                var value = prop.GetValue(command);
                if (value != null)
                    signUpRequest.UserAttributes.Add(new AttributeType()
                    {
                        Name = prop.Name.ToLower(),
                        Value = value.ToString()
                    });
            }

            try
            {
                var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.SignUpAsync(signUpRequest, cancellationToken));

                return new Model.SignUpResponse {Confirmed = response.UserConfirmed, UUID = response.UserSub};
            }
            catch (Exception ex)
            {
                throw CatchException(ex);
            }
        }


        public virtual async Task ConfirmSignUp(Model.ConfirmSignUpRequest request,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(request, nameof(request));

            request.EnsureIsValid();

            var confirmSignUpRequest = new Amazon.CognitoIdentityProvider.Model.ConfirmSignUpRequest()
            {
                ClientId = clientId,
                Username = request.UserName.ToLower(),
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
                    await CognitoidentityProvider.ConfirmSignUpAsync(confirmSignUpRequest, cancellationToken));
            }
            catch (Exception ex)
            {
                throw CatchException(ex);
            }
        }

        public virtual async Task ResendConfirmationCode(Model.ResendConfirmationCodeRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var resendCodeRequest = new Amazon.CognitoIdentityProvider.Model.ResendConfirmationCodeRequest()
            {
                ClientId = clientId,
                Username = command.UserName.ToLower(),
                UserContextData = new UserContextDataType
                {
                    EncodedData =
                        $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
                }
            };

            try
            {
                await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.ResendConfirmationCodeAsync(resendCodeRequest, cancellationToken));
            }
            catch (Exception ex)
            {
                throw CatchException(ex);
            }
        }

        public virtual async Task ChangePassword(Model.ChangePasswordRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var changeRequest = new Amazon.CognitoIdentityProvider.Model.ChangePasswordRequest()
            {
                AccessToken = command.AccessToken,
                ProposedPassword = command.ProposedPassword,
                PreviousPassword = command.PreviousPassword
            };

            try
            {
                await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.ChangePasswordAsync(changeRequest, cancellationToken));
            }
            catch (Exception ex)
            {
                throw CatchException(ex);
            }
        }

        public virtual async Task<T> GetUser<T>(Model.GetUserRequest request,
            CancellationToken cancellationToken = default) where T : IGetUserResponse
        {
            Check.NotNull(request, nameof(request));

            request.EnsureIsValid();

            var listUserRequest = new ListUsersRequest
            {
                UserPoolId = userPoolId,
                Filter = $"{request.Field}=\"{request.Value}\""
            };

            try
            {
                var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.ListUsersAsync(listUserRequest, cancellationToken));

                var cognitoUser = response?.Users.FirstOrDefault(u =>
                    request.ExcludeExternalUser && u.UserStatus != "EXTERNAL_PROVIDER");

                if (cognitoUser == null)
                    return default;

                var user = Activator.CreateInstance<T>();

                user.UserName = cognitoUser.Username;
                user.Status = cognitoUser.UserStatus.ToString();
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

                            if (propInfo != null) propInfo.SetValue(user, userAttribute.Value);

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
            Model.RespondToAuthChallengeRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var challengeResponses = new Dictionary<string, string>()
            {
                {"USERNAME", command.UserName.ToLower()}
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
                    await CognitoidentityProvider.RespondToAuthChallengeAsync(request, cancellationToken));

                var result = new AuthChallengeResponse();

                if (response.ResponseMetadata != null) result.Metadata = response.ResponseMetadata.Metadata;

                if (response.AuthenticationResult != null)
                    result.AuthenticationResult = new SignInResponse()
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

        public async Task ConfirmForgotPassword(Model.ConfirmForgotPasswordRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            try
            {
                var respond = new Amazon.CognitoIdentityProvider.Model.ConfirmForgotPasswordRequest()
                {
                    ClientId = clientId,
                    Username = command.UserName.ToLower(),
                    Password = command.Password,
                    ConfirmationCode = command.ConfirmationCode,
                    UserContextData = new UserContextDataType
                    {
                        EncodedData =
                            $"IP:{command.IpAddress};ServerPath:{command.ServerPath};ServerName:{command.ServerName}"
                    }
                };

                await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.ConfirmForgotPasswordAsync(respond, cancellationToken));
            }
            catch (Exception ex)
            {
                CatchException(ex);
            }
        }

        public virtual async Task<OAuth2SignInResponse> SocialSignIn(SocialSignInRequest command,
            CancellationToken cancellationToken)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            OAuth2SignInResponse response = null;

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("code", command.Code),
                new KeyValuePair<string, string>("redirect_uri", command.RedirectUri)
            };

            try
            {
                var uri = domainEndPoint.AppendResourceUri("oauth2/token");

                using var httpClient = new HttpClient();

                var responseMessage =
                    await httpClient.PostAsync(uri, new FormUrlEncodedContent(parameters), cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                    throw new BusinessException(ErrorCode.OAuthResponseError);

                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                response = JsonSerializer.Deserialize<OAuth2SignInResponse>(responseContent);

                if (response == null)
                    throw new BusinessException(ErrorCode.OAuthResponseError);

                var socialUser = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await CognitoidentityProvider.GetUserAsync(
                        new Amazon.CognitoIdentityProvider.Model.GetUserRequest {AccessToken = response.AccessToken},
                        cancellationToken));

                if (socialUser == null)
                    throw new CriticalException(ErrorCode.UserNotFound);

                var socialUserEmail = GetUserAttributeValue(socialUser.UserAttributes, "email");

                var listUsersResponse = await CognitoidentityProvider.ListUsersAsync(new ListUsersRequest
                {
                    UserPoolId = userPoolId,
                    Filter = $"email=\"{socialUserEmail}\""
                }, cancellationToken);

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
            catch (Exception e)
            {
                CatchException(e);
            }

            return response;
        }

        public virtual async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest command,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(command, nameof(command));

            command.EnsureIsValid();

            var authRequest = new InitiateAuthRequest()
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
                    await CognitoidentityProvider.InitiateAuthAsync(authRequest, cancellationToken));

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

        protected override void DisposeServices()
        {
            cognitoidentityProvider?.Dispose();
        }
    }
}