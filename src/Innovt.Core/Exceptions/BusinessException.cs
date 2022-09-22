// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Innovt.Core.Exceptions;

[Serializable]
public class BusinessException : BaseException, ISerializable
{
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

    public BusinessException(IList<ErrorMessage> errors) : base(CreateMessage(errors))
    {
        Errors = errors;
    }

    public BusinessException(ErrorMessage[] errors) : base(CreateMessage(errors))
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


    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException(nameof(info));

        info.AddValue("Code", Code);
        info.AddValue("Errors", Errors, typeof(IEnumerable<ErrorMessage>));

        base.GetObjectData(info, context);
    }

    public string ReadFullErrors()
    {
        if (Errors is null || !Errors.Any())
            return Message;

        return string.Join(",", Errors.Select(p => p.Message));
    }

    private static string CreateMessage(IEnumerable<ErrorMessage> errors)
    {
        var errorMessages = errors as ErrorMessage[] ?? errors.ToArray();

        if (!errorMessages.Any()) return string.Empty;
        
        var strError = new StringBuilder();
        
        strError.Append("[");
        
        var properties = errorMessages.GroupBy(err => err.PropertyName);

        foreach (var property in properties)
        {
            strError.Append("{" + $"\"Property\":\"{property.Key}\"");

            var propertyErrors = errorMessages.Where(p => p.PropertyName == property.Key);
            strError.Append("[");
            foreach (var propertyError in propertyErrors)
            {
                strError.Append("{" + $"\"Message\":\"{propertyError.Message}\",\"Code\":\"{propertyError.Code}\"" +
                                "},");
            }

            strError.Append("]");
        }

        strError.Append("]");

        return strError.ToString();
    }
}