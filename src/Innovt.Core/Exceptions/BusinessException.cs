// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Innovt.Core.Exceptions;

[Serializable]
/// <summary>
/// Represents an exception that is thrown when a business rule is violated or validation errors occur.
/// </summary>
public class BusinessException : BaseException, ISerializable
{
    private const string DefaultValidationMessage = "One or more validation errors occurred.";

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BusinessException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with a specified error message
    ///     and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="ex">
    ///     The exception that is the cause of the current exception, or a null reference if no inner exception is
    ///     specified.
    /// </param>
    public BusinessException(string message, Exception ex) : base(message, ex)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with a code and a specified error message.
    /// </summary>
    /// <param name="code">A code associated with the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BusinessException(string code, string message) : base(message)
    {
        Code = code;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with a code, a specified error message,
    ///     and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="code">A code associated with the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="ex">
    ///     The exception that is the cause of the current exception, or a null reference if no inner exception is
    ///     specified.
    /// </param>
    public BusinessException(string code, string message, Exception ex) : base(message, ex)
    {
        Code = code;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with a list of validation errors.
    /// </summary>
    /// <param name="errors">A list of validation error messages.</param>
    public BusinessException(IList<ErrorMessage> errors) : base(DefaultValidationMessage)
    {
        Errors = errors;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with an array of validation errors.
    /// </summary>
    /// <param name="errors">An array of validation error messages.</param>
    public BusinessException(ErrorMessage[] errors) : base(DefaultValidationMessage)
    {
        Errors = errors;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with a single validation error.
    /// </summary>
    /// <param name="error">A single validation error message.</param>
    public BusinessException(ErrorMessage error) : this(new[] { error })
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with serialized data.
    /// </summary>
    /// <param name="serializationInfo">
    ///     The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the
    ///     serialized object data about the exception being thrown.
    /// </param>
    /// <param name="streamingContext">
    ///     The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains
    ///     contextual information about the source or destination.
    /// </param>
    protected BusinessException(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
        Code = serializationInfo?.GetString("ResourceName");
        Errors = (IEnumerable<ErrorMessage>)serializationInfo?.GetValue("Errors", typeof(IEnumerable<ErrorMessage>));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BusinessException" /> class with no specified error message.
    /// </summary>
    public BusinessException()
    {
    }

    /// <summary>
    ///     Gets or sets a code associated with the exception.
    /// </summary>
    public string Code { get; protected set; }

    /// <summary>
    ///     Gets or sets a list of validation error messages.
    /// </summary>
    public IEnumerable<ErrorMessage> Errors { get; set; }

    /// <summary>
    ///     Gets the detail information associated with the exception.
    /// </summary>
    public object Detail => CreateMessage(Errors);

    /// <summary>
    ///     Reads the full validation error messages.
    /// </summary>
    /// <returns>A concatenated string of validation error messages.</returns>
    public string ReadFullErrors()
    {
        if (Errors is null || !Errors.Any())
            return Message;

        return string.Join(",", Errors.Select(p => p.Message));
    }

    /// <summary>
    ///     Creates a structured error message from the provided collection of <see cref="ErrorMessage" /> objects.
    /// </summary>
    /// <param name="errors">The collection of <see cref="ErrorMessage" /> objects to create the message from.</param>
    /// <returns>
    ///     A structured error message that groups errors by property name and includes error codes and messages for each
    ///     property.
    ///     Returns null if the input collection is null or empty.
    /// </returns>
    private object CreateMessage(IEnumerable<ErrorMessage> errors)
    {
        if (errors is null) return null;

        var errorMessages = errors.ToArray();

        if (!errorMessages.Any()) return null;

        var errorResult = (from error in errorMessages
            group error by error.PropertyName
            into errorGrouped
            orderby errorGrouped.Key
            select new
            {
                Property = errorGrouped.Key,
                Errors = from e in errorMessages
                    where e.PropertyName == errorGrouped.Key
                    select new
                    {
                        e.Code, e.Message
                    }
            }).ToList();

        return errorResult;
    }
}