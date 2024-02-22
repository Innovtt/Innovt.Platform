// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Provides a collection of error codes used in the application.
/// </summary>
public static class ErrorCode
{
    /// <summary>
    ///     Gets the error code for "UsernameAlreadyExists".
    /// </summary>
    public static string UsernameAlreadyExists => "UsernameAlreadyExists";

    /// <summary>
    ///     Gets the error code for "NotAuthorized".
    /// </summary>
    public static string NotAuthorized => "NotAuthorized";

    /// <summary>
    ///     Gets the error code for "TooManyRequests".
    /// </summary>
    public static string TooManyRequests => "TooManyRequests";

    /// <summary>
    ///     Gets the error code for "PasswordResetRequired".
    /// </summary>
    public static string PasswordResetRequired => "PasswordResetRequired";

    /// <summary>
    ///     Gets the error code for "UserNotFound".
    /// </summary>
    public static string UserNotFound => "UserNotFound";

    /// <summary>
    ///     Gets the error code for "UserNotConfirmed".
    /// </summary>
    public static string UserNotConfirmed => "UserNotConfirmed";

    /// <summary>
    ///     Gets the error code for "InvalidPassword".
    /// </summary>
    public static string InvalidPassword => "InvalidPassword";

    /// <summary>
    ///     Gets the error code for "CodeMismatch".
    /// </summary>
    public static string CodeMismatch => "CodeMismatch";

    /// <summary>
    ///     Gets the error code for "ExpiredCode".
    /// </summary>
    public static string ExpiredCode => "ExpiredCode";

    /// <summary>
    ///     Gets the error code for "LimitExceeded".
    /// </summary>
    public static string LimitExceeded => "LimitExceeded";

    /// <summary>
    ///     Gets the error code for "InternalServerError".
    /// </summary>
    public static string InternalServerError => "InternalServerError";

    /// <summary>
    ///     Gets the error code for "OAuthResponseError".
    /// </summary>
    public static string OAuthResponseError => "OAuthResponseError";

    /// <summary>
    ///     Gets the error code for "ChallengeNotAvailable".
    /// </summary>
    public static string ChallengeNotAvailable => "ChallengeNotAvailable";
}