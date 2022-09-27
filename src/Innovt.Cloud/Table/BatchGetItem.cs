﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;

public class BatchGetItem
{
    public bool ConsistentRead { get; set; }

    public Dictionary<string, string> ExpressionAttributeNames { get; set; }

    public List<Dictionary<string, object>> Keys { get; set; }

    public string ProjectionExpression { get; set; }
}