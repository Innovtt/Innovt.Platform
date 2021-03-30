using System;
using System.Collections.Generic;
using System.Text;

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