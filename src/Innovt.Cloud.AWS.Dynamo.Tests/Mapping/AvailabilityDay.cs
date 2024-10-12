using System;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class AvailabilityDay
{
    public List<int> AvailableDays { get; set; }
    public TimeOnly StartTime { get; set; }
    
    public TimeOnly EndingTime { get; set; }
}