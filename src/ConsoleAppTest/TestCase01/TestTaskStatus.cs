using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Domain.Core.Model;

namespace ConsoleAppTest.TestCase01;

public class TestTaskStatus : ValueObject
{
    [DynamoDBIgnore] public static readonly TestTaskStatus New = new(1, "New");
    [DynamoDBIgnore] public static readonly TestTaskStatus Running = new(2, "Running");
    [DynamoDBIgnore] public static readonly TestTaskStatus Finished = new(3, "Finished");
    [DynamoDBIgnore] public static readonly TestTaskStatus Error = new(4, "Error");
    [DynamoDBIgnore] public static readonly TestTaskStatus Canceled = new(5, "Canceled");
    public string Name { get; set; }

    private static IList<TestTaskStatus> _all = new List<TestTaskStatus>()
    {
        New,
        Running,
        Finished,
        Error,
        Canceled
    };

    public TestTaskStatus()
    {
    }

    public TestTaskStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static IList<TestTaskStatus> FindAll()
    {
        return _all;
    }

    public static TestTaskStatus GetByPk(int id)
    {
        return _all.SingleOrDefault(m => m.Id == id);
    }
}