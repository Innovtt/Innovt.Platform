using Innovt.Core.Exceptions;

namespace Innovt.Cloud.Scheduler;
#pragma warning disable CA1032 // Implement standard exception constructors

public class ScheduleConflictException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
{
    public ScheduleConflictException(string message) : base(message)
    {
    }
}