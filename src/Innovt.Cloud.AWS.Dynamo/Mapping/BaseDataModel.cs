using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping;

public abstract class BaseDataModel<TDataModel, TDomain> : ITableMessage where TDataModel : class where TDomain : class
{
    [DynamoDBHashKey("PK")] public string Pk { get; set; }

    [DynamoDBRangeKey("SK")] public string Sk { get; set; }

    [DynamoDBProperty]
    public string EntityType
    {
        get => GetEntityType(); // We have to keep it because of the DyanmoDB SDK
        set => _ = value;
    }

    public string Id { get; set; }

    protected abstract string GetEntityType();

    public TDataModel ToDataModel(TDomain domain)
    {
        if (domain == null) return null;

        var model = SimpleMapper.Map<TDomain, TDataModel>(domain);

        CustomDataModelMap(model, domain);

        return model;
    }

    public IList<TDataModel> ToDataModel(IList<TDomain> domains)
    {
        return domains?.Select(ToDataModel).ToList();
    }

    public TDomain ToDomain(TDataModel dataModel)
    {
        if (dataModel == null) return null;

        var tDomain = SimpleMapper.Map<TDataModel, TDomain>(dataModel);

        CustomDomainMap(tDomain, dataModel);

        return tDomain;
    }

    public IList<TDomain> ToDomain(IList<TDataModel> dataModels)
    {
        return dataModels?.Select(ToDomain).ToList();
    }

    protected abstract void CustomDataModelMap(TDataModel dataModel, TDomain domain);

    protected abstract void CustomDomainMap(TDomain tDomain, TDataModel dataModel);
}