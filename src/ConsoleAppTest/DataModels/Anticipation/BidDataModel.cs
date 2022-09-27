// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using System;

namespace ConsoleAppTest.DataModels.Anticipation;

public class BidDataModel : BaseDataModel
{
    public BidDataModel()
    {
        EntityType = "Bid";
    }

    public Guid SupplierId { get; set; }
    public Guid BuyerId { get; set; }
    public Guid UserId { get; set; }
    public int StrategyId { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal? RequestedRate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Attempt { get; set; }
    public int RemainingAttempt { get; set; }
    public bool HasValidOffer { get; set; }
    public string NoOfferReason { get; set; }

    public string EntityTypeCreatedAt
    {
        get => $"ET#{EntityType}#CREATED#{CreatedAt:yyyy-MM-ddTHH:mm:ss.000Z}";
        set => _ = value;
    }
}