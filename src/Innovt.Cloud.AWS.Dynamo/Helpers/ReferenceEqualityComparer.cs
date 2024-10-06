using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Helpers;

internal sealed class ReferenceEqualityComparer : IEqualityComparer<object>
{
    public new bool Equals(object x, object y) => ReferenceEquals(x, y);

    public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
}