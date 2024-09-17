// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PreSignup;

/// <summary>
///     Represents a response for post-confirmation actions.
/// </summary>
public class PreSignupResponse : TriggerResponse
{
    /// <summary>
    ///     Set to true to auto-confirm the user, or false otherwise.
    /// </summary>
    public bool AutoConfirmUser { get; set; }

    /// <summary>
    ///     Set to true to set as verified the email of a user who is signing up, or false otherwise. If autoVerifyEmail is set
    ///     to true, the email attribute must have a valid, non-null value. Otherwise an error will occur and the user will not
    ///     be able to complete sign-up.
    /// </summary>
    public bool AutoVerifyPhone { get; set; }

    /// <summary>
    ///     Set to true to set as verified the phone number of a user who is signing up, or false otherwise. If autoVerifyPhone
    ///     is set to true, the phone_number attribute must have a valid, non-null value. Otherwise an error will occur and the
    ///     user will not be able to complete sign-up.
    /// </summary>
    public bool AutoVerifyEmail { get; set; }
}