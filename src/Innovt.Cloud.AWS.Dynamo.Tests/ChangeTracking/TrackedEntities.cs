using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Dynamo.Tests.ChangeTracking;

public class ScalarEntity
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

public enum Priority
{
    Low,
    Medium,
    High
}

public class ComplexEntity
{
    public string Id { get; set; } = string.Empty;
    public Address Home { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<LineItem> Lines { get; set; } = new();
}

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int ZipCode { get; set; }
}

public class LineItem
{
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class TreeNode
{
    public string Name { get; set; } = string.Empty;
    public TreeNode? Parent { get; set; }
    public List<TreeNode>? Children { get; set; }
}
