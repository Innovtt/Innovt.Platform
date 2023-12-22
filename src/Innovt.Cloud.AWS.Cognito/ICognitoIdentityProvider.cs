// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Cognito.Model;

namespace Innovt.Cloud.AWS.Cognito;

public interface ICognitoIdentityProvider
{
    /// <summary>
    ///     Change the password for an authenticated user.
    /// </summary>
    /// <param name="command">The request object containing the necessary information to change the user's password.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task ChangePassword(ChangePasswordRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Confirm a forgot password request and set a new password for the user.
    /// </summary>
    /// <param name="command">The request object containing the confirmation details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task ConfirmForgotPassword(ConfirmForgotPasswordRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Confirm a user's sign-up by verifying the confirmation code.
    /// </summary>
    /// <param name="request">The request object containing the user's sign-up confirmation details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task ConfirmSignUp(ConfirmSignUpRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Initiate a forgot password request for a user.
    /// </summary>
    /// <param name="command">The request object containing the user's information.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task ForgotPassword(ForgotPasswordRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve user information based on the specified request.
    /// </summary>
    /// <typeparam name="T">
    ///     The response object type containing user information. Must implement the IGetUserResponse
    ///     interface.
    /// </typeparam>
    /// <param name="request">The request object containing the criteria to retrieve user information.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing user information.</returns>
    Task<T> GetUser<T>(GetUserRequest request, CancellationToken cancellationToken = default)
        where T : IGetUserResponse;

    /// <summary>
    ///     Retrieve user information based on the access token.
    /// </summary>
    /// <typeparam name="T">
    ///     The response object type containing user information. Must implement the IGetUserResponse
    ///     interface.
    /// </typeparam>
    /// <param name="accessToken">A valid access token.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing user information.</returns>
    Task<T> GetUser<T>(string accessToken, CancellationToken cancellationToken = default) where T : IGetUserResponse;


    /// <summary>
    ///     Refresh the user's authentication token.
    /// </summary>
    /// <param name="command">The request object containing the refresh token.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing the refreshed authentication token.</returns>
    Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Resend a confirmation code to the user's email or phone number.
    /// </summary>
    /// <param name="command">The request object containing the user's information.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task ResendConfirmationCode(ResendConfirmationCodeRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Respond to an authentication challenge during the sign-in process.
    /// </summary>
    /// <param name="command">The request object containing the challenge response.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing the result of the authentication challenge.</returns>
    Task<AuthChallengeResponse> RespondToAuthChallenge(RespondToAuthChallengeRequest command,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Authenticate a user using one-time password (OTP) credentials.
    /// </summary>
    /// <param name="command">The request object containing the OTP credentials.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing the authentication result.</returns>
    Task<SignInResponse> SignIn(OtpSignInRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Authenticate a user using standard credentials (username and password).
    /// </summary>
    /// <param name="command">The request object containing the standard sign-in credentials.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing the authentication result.</returns>
    Task<SignInResponse> SignIn(SignInRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sign a user out of the application.
    /// </summary>
    /// <param name="request">The request object containing the access token.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task SignOut(SignOutRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sign up a new user.
    /// </summary>
    /// <param name="command">The request object containing the user's sign-up details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing the sign-up confirmation details.</returns>
    Task<SignUpResponse> SignUp(ISignUpRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Authenticate a user using social media credentials.
    /// </summary>
    /// <param name="command">The request object containing the social media sign-in credentials.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The response object containing the social media sign-in result.</returns>
    Task<OAuth2SignInResponse> SocialSignIn(SocialSignInRequest command, CancellationToken cancellationToken);

    /// <summary>
    ///     Update user attributes with new values.
    /// </summary>
    /// <param name="command">The request object containing the updated user attributes.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task UpdateUserAttributes(UpdateUserAttributesRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     This method allow you to update user attributes without sending a token. Internally we are calling Admin Update
    ///     User Attributes.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateUserAttributes(AdminUpdateUserAttributesRequest command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Link existent user account with a social user account to avoid billing issues and other problems.
    /// </summary>
    /// <param name="command">The request object containing the user pool and email from user.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    Task LinkSocialUser(LinkSocialAccountRequest command, CancellationToken cancellationToken = default);
}