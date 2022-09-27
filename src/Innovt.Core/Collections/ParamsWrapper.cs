// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.Collections;

public class ParamsWrappers<T>
{
    public ParamsWrappers(params T[] parameters)
    {
        Parameters = parameters;
    }

    public T[] Parameters { get; set; }
}