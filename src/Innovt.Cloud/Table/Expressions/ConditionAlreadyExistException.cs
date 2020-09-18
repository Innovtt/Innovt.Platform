
namespace Innovt.Cloud.Table
{
    public class ConditionAlreadyExistException: Innovt.Core.Exceptions.BaseException
    {
        public ConditionAlreadyExistException(FilterCondition condition):base($"Condition with attribute {condition.AttributeName} already exist.")
        {

        }
    }
}
