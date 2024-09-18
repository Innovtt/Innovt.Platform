// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo.Tests

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using Amazon.DynamoDBv2.Model;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
public class DynamoHelperTests
{
    private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(ExpandoObject filter,
        string attributes)
    {
        if (filter == null)
            return new Dictionary<string, AttributeValue>();

        var attributeValues = new Dictionary<string, AttributeValue>();

        var properties = filter as IDictionary<string, object>;

        if (properties.Count <= 0)
            return attributeValues;

        foreach (var (s, value) in properties)
        {
            var key = $":{s}".ToLower(CultureInfo.CurrentCulture);

            if (!attributes.Contains(key, StringComparison.InvariantCultureIgnoreCase) ||
                attributeValues.ContainsKey(key)) continue;

            attributeValues.Add(key, value == null ? new AttributeValue { NULL = true } : new AttributeValue());
        }

        return attributeValues;
    }

    [Test]
    public void CreateExpressionAttributeValues()
    {
        /* var filter = new
         {
             pk = "a",
             sk = "ss",
             ids = new int[] { 1, 2, 3 }
         };*/

        var filter = new ExpandoObject();
        filter.TryAdd("pk", "a");
        filter.TryAdd("sk", "a");
        filter.TryAdd("ids", "1,2,3");

        var result = CreateExpressionAttributeValues(filter, ":pk,:sk,:ids");


        Assert.That(result, Is.Not.Null);
    }
}