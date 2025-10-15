using System;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

public class Organization
{
    public int SectorId { get; set; }
    public int TimeZoneId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid? BillingAddressId { get; set; }
    public Guid? ShippingAddressId { get; set; }
}