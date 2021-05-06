// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public static class ErrorCode
    {
        public static string UsernameAlreadyExists => "UsernameAlreadyExists";
        public static string NotAuthorized => "NotAuthorized";
        public static string TooManyRequests => "TooManyRequests";
        public static string PasswordResetRequired => "PasswordResetRequired";
        public static string UserNotFound => "UserNotFound";
        public static string UserNotConfirmed => "UserNotConfirmed";
        public static string InvalidPassword => "InvalidPassword";
        public static string CodeMismatch => "CodeMismatch";
        public static string ExpiredCode => "ExpiredCode";
        public static string LimitExceeded => "LimitExceeded";
        public static string InternalServerError => "InternalServerError";
        public static string OAuthResponseError => "OAuthResponseError";
        public static string ChallengeNotAvailable => "ChallengeNotAvailable";
    }
}