// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using System;
using System.Collections.Generic;

namespace ConsoleAppTest.DataModels.CapitalSource.DataModels;

public class ContractDataModel : CapitalSourceBaseDataModel
{
    public ContractDataModel()
    {
    }

    public Guid RootId { get; set; }
    public ContractParametersDataModel Parameters { get; set; }
    public List<string> BuyersIds { get; set; }
    public List<ContractBuyerDataModel> Buyers { get; set; }
    public bool Deleted { get; set; }
    public bool Canceled { get; set; }
    public Guid? PreviousContractId { get; set; }
    public decimal CreditLimit { get; set; }

    public string EconomicGroupContract { get; set; }

    public Guid EconomicGroupId { get; set; }
    public decimal Balance { get; set; }

    public DateTime BalanceUpdatedAt { get; set; }

    public Guid CapitalSourceId { get; set; }

    public Guid? AssignmentTermFileId { get; set; }

    public List<AssignmentTermFileDataModel> AssignmentTermFileTemplateHistory { get; set; }

    public int StatusId { get; set; }

    public List<ContractStatusChangeDataModel> StatusChanges { get; set; }

    //public ContractStatusChangeDataModel[] StatusChanges { get; set; }
    public bool SuspendedByEconomicGroup { get; set; }
    public bool SuspendedByCapitalSource { get; set; }
    public string ContractNumber { get; set; }
    public string ParameterUpdates { get; set; }
}