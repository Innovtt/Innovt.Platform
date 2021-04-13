// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Cognito.Model;

namespace Innovt.Cloud.AWS.Cognito
{
    public interface ICognitoIdentityProvider
    {
        Task ChangePassword(ChangePasswordRequest command, CancellationToken cancellationToken = default);
        Task ConfirmForgotPassword(ConfirmForgotPasswordRequest command, CancellationToken cancellationToken = default);
        Task ConfirmSignUp(ConfirmSignUpRequest request, CancellationToken cancellationToken = default);
        Task ForgotPassword(ForgotPasswordRequest command, CancellationToken cancellationToken = default);

        Task<T> GetUser<T>(GetUserRequest request, CancellationToken cancellationToken = default)
            where T : IGetUserResponse;

        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest command,
            CancellationToken cancellationToken = default);

        Task ResendConfirmationCode(ResendConfirmationCodeRequest command,
            CancellationToken cancellationToken = default);

        Task<AuthChallengeResponse> RespondToAuthChallenge(RespondToAuthChallengeRequest command,
            CancellationToken cancellationToken = default);

        Task<SignInResponse> SignIn(OtpSignInRequest command, CancellationToken cancellationToken = default);
        Task<SignInResponse> SignIn(SignInRequest command, CancellationToken cancellationToken = default);
        Task SignOut(SignOutRequest request, CancellationToken cancellationToken = default);
        Task<SignUpResponse> SignUp(ISignUpRequest command, CancellationToken cancellationToken = default);
        Task<OAuth2SignInResponse> SocialSignIn(SocialSignInRequest command, CancellationToken cancellationToken);
        Task UpdateUserAttributes(UpdateUserAttributeRequest command, CancellationToken cancellationToken = default);
    }
}