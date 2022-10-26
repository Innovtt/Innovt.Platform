using System;
using System.Dynamic;
using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ConsoleAppTest.TestCase01
{
    public class TestParametersConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            var data = "";

            var parameters = value as ITestParameters;

            if (parameters == null) throw new ArgumentOutOfRangeException();

            if (parameters is TestAnticipationRequestParametersIntegration anticipationRequestParametersIntegration)
                data = JsonSerializer.Serialize(
                    anticipationRequestParametersIntegration);
            else if (parameters is TestFindSupplierParametersIntegration supplierParametersIntegration)
                data = JsonSerializer.Serialize(supplierParametersIntegration);
            else if (parameters is TestFindFinancialDocumentParametersIntegration financialDocumentParametersIntegration)
                data = JsonSerializer.Serialize(
                    financialDocumentParametersIntegration);
            else
                throw new ArgumentOutOfRangeException("TestMichel");

            DynamoDBEntry entry = new Primitive
            {
                Value = data
            };
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            ITestParameters parameters = null;

            var primitive = entry as Primitive;
            if (primitive == null) return null;

            if (primitive.Type != DynamoDBEntryType.String) throw new InvalidCastException();

            if (!(primitive.Value is string) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException("TestMichel");

            var value = (string)primitive.Value;

            dynamic json = JsonSerializer.Deserialize<ExpandoObject>(value, new JsonSerializerOptions() { });

            var type = $"{json.Type}".ToString();

            switch (type)
            {
                case "FIND_FINANCIAL_DOCUMENT":
                    parameters = JsonSerializer.Deserialize<TestFindFinancialDocumentParametersIntegration>(value);
                    break;

                case "FIND_SUPPLIER":
                    parameters = JsonSerializer.Deserialize<TestFindSupplierParametersIntegration>(value);
                    break;

                case "ANTICIPATE_PAYMENTS":
                    parameters = JsonSerializer.Deserialize<TestAnticipationRequestParametersIntegration>(value);
                    break;
            }

            return parameters;
        }
    }
}