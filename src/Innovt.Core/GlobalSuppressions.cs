// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "<Pending>",
        Scope = "member", Target = "~M:Innovt.Core.Attributes.ModelExcludeFilterAttribute.#ctor(System.String)")]
[assembly:
    SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>",
        Scope = "type", Target = "~T:Innovt.Core.Collections.PagedCollection`1")]
[assembly:
    SuppressMessage("Design", "CA1055:URI-like return values should not be strings", Justification = "<Pending>",
        Scope = "member", Target = "~M:Innovt.Core.Utilities.StringExtensions.UrlEncode(System.String)~System.String")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "<Pending>", Scope = "member", Target = "~M:Innovt.Core.CrossCutting.Log.ILogger.Error(System.String,System.Object[])")]
[assembly: SuppressMessage("Usage", "CA2229:Implement serialization constructors", Justification = "<Pending>", Scope = "type", Target = "~T:Innovt.Core.Exceptions.BusinessException")]
