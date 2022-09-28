// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using Innovt.Core.Collections;

namespace Innovt.Core.Exceptions;

[Serializable]
public class BusinessException : BaseException, ISerializable
{
    private const string DefaultValidationMessage = "One or more validation errors occurred.";
    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string message, Exception ex) : base(message, ex)
    {
    }


    public BusinessException(string code, string message) : base(message)
    {
        Code = code;
    }

    public BusinessException(string code, string message, Exception ex) : base(message, ex)
    {
        Code = code;
    }

    public BusinessException(IList<ErrorMessage> errors) : base(DefaultValidationMessage)
    {
        Errors = errors;
    }

    public BusinessException(ErrorMessage[] errors) : base(DefaultValidationMessage)
    {
        Errors = errors;
    }

    public BusinessException(ErrorMessage error) : this(new[] { error })
    {
    }

    protected BusinessException(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
        Code = serializationInfo?.GetString("ResourceName");
        Errors = (IEnumerable<ErrorMessage>)serializationInfo?.GetValue("Errors", typeof(IEnumerable<ErrorMessage>));
    }

    public BusinessException()
    {
    }

    public string Code { get; protected set; }
    public IEnumerable<ErrorMessage> Errors { get; set; }
    public object Detail => CreateMessage(Errors);

    public string ReadFullErrors()
    {
        if (Errors is null || !Errors.Any())
            return Message;

        return string.Join(",", Errors.Select(p => p.Message));
    }

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
                        Code = e.Code,
                        Message = e.Message,
                    }
            }).ToList();

        return errorResult;
    }
}