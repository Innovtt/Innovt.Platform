// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo.Tests

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
public class HelperTests
{
    public HelperTests()
    {
        
    }
    
    private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(ExpandoObject filter, string attributes)
    {
        if (filter == null)
            return new Dictionary<string, AttributeValue>();

        var attributeValues = new Dictionary<string, AttributeValue>();

        var properties = filter as IDictionary<string, object>;
                
        if (properties.Count <= 0)
            return attributeValues;

        foreach (var item in properties)
        {
            var key = $":{item.Key}".ToLower(CultureInfo.CurrentCulture);

            if (attributes.Contains(key, StringComparison.InvariantCultureIgnoreCase) &&
                !attributeValues.ContainsKey(key))
            {
                var value = item.Value;
                attributeValues.Add(key, value == null ? new AttributeValue {NULL = true} : new AttributeValue());
            }   
        }

        return attributeValues;
    }
    private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(object filter, string attributes)
    {
        if (filter == null)
            return new Dictionary<string, AttributeValue>();

        var attributeValues = new Dictionary<string, AttributeValue>();

        var properties = filter.GetType().GetProperties();
                
        if (properties.Length <= 0 && filter is ExpandoObject expando) 
            return CreateExpressionAttributeValues(expando, attributes);

        foreach (var item in properties)
        {
            var key = $":{item.Name}".ToLower(CultureInfo.CurrentCulture);

            if (attributes.Contains(key, StringComparison.InvariantCultureIgnoreCase) &&
                !attributeValues.ContainsKey(key))
            {
                var value = item.GetValue(filter);
                attributeValues.Add(key, value == null ? new AttributeValue {NULL = true} : new AttributeValue());
            }   
        }

        return attributeValues;
    }
    
    [Test]

    public void DDD()
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
        
        
        

    }


}