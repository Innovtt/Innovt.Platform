using Innovt.Core.Exceptions;

namespace Innovt.Cloud.Scheduler;
#pragma warning disable CA1032 // Implement standard exception constructors

public class ScheduleNotFoundException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
{
    public ScheduleNotFoundException(string message) : base(message)
    {
    }
}