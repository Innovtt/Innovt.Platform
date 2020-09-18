using Innovt.Core.Cqrs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Cloud.Table
{
    public class BaseRequest
    {
        public string IndexName { get; set; }
        
        public IFilter Filter { get; set; }

        public List<string> AttributesToGet { get; set; }
        public int PageSize { get; set; }
        public string PaginationToken { get; set; }
        public string FilterExpression { get; set; }

        //ExpressionAttributeValues
        //public bool ConditionExist(FilterCondition condition)
        //{
        //    if (condition is null)
        //    {
        //        throw new System.ArgumentNullException(nameof(condition));
        //    }

        //    if (Filter == null)
        //        return false;

        //    return Filter.Any(c => c.AttributeName == condition.AttributeName);
        //}

        //public QueryRequest AddCondition(string attributeName, ComparisonOperator compararionOperator, params string[] values)
        //{
        //    return AddCondition(new FilterCondition(attributeName, compararionOperator, values));
        //}

        //public QueryRequest AddCondition(FilterCondition condition)
        //{
        //    if (condition is null)
        //    {
        //        throw new System.ArgumentNullException(nameof(condition));
        //    }

        //    if (Filter == null)
        //        Filter = new List<FilterCondition>();

        //    if (ConditionExist(condition))
        //        throw new ConditionAlreadyExistException(condition);

        //    Filter.Add(condition);

        //    return this;
        //}

        //public QueryRequest RemoveCondition(FilterCondition condition)
        //{
        //    if (condition is null)
        //    {
        //        throw new System.ArgumentNullException(nameof(condition));
        //    }

        //    if (ConditionExist(condition))
        //    {
        //        var exCondition = Filter.SingleOrDefault(c => c.AttributeName == condition.AttributeName);

        //        Filter.Remove(exCondition);
        //    }

        //    return this;
        //}
    }
}