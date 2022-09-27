// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.Model;

public class ResponseError
{
    public string TraceId { get; set; }

    public string Message { get; set; }

    public string Code { get; set; }

    public object Detail { get; set; }
}