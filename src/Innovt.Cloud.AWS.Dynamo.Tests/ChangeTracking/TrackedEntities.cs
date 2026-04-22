using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Dynamo.Tests.ChangeTracking;

internal sealed class ScalarEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Counter { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid Token { get; set; }
    public bool IsEnabled { get; set; }
    public Priority Level { get; set; }

    [JsonIgnore]
    public string Ephemeral { get; set; } = string.Empty;
}

internal enum Priority
{
    Low,
    Medium,
    High
}

internal sealed class ComplexEntity
{
    public string Id { get; set; } = string.Empty;
    public Address Home { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<LineItem> Lines { get; set; } = new();
}

internal sealed class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int ZipCode { get; set; }
}

internal sealed class LineItem
{
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

internal sealed class TreeNode
{
    public string Name { get; set; } = string.Empty;
    public TreeNode? Parent { get; set; }
    public List<TreeNode>? Children { get; set; }
}
