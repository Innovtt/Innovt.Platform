using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;
using Innovt.Core.Utilities.Mapper;
// ReSharper disable MemberCanBeProtected.Global

namespace Innovt.Cloud.AWS.Dynamo.Mapping;

/// <summary>
///     Base abstract class representing a data model that can be mapped to/from a domain model.
/// </summary>
/// <typeparam name="TDataModel">The type of the data model.</typeparam>
/// <typeparam name="TDomain">The type of the domain model.</typeparam>
public abstract class BaseDataModel<TDataModel, TDomain> : ITableMessage where TDataModel : class where TDomain : class
{
    /// <summary>
    ///     Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    ///     Gets or sets the partition key for DynamoDB.
    /// </summary>
    [DynamoDBHashKey("PK")]
    public string Pk { get; set; }

    /// <summary>
    ///     Gets or sets the sort key for DynamoDB.
    /// </summary>
    [DynamoDBRangeKey("SK")]
    public string Sk { get; set; }
    /// <summary>
    ///     Gets or sets the entity type.
    /// </summary>
    [DynamoDBProperty]
    public string EntityType
    {
        get => GetEntityType(); // We have to keep it because of the DyanmoDB SDK
        set => _ = value;
    }

    /// <summary>
    ///     Protected abstract method to get the entity type.
    /// </summary>
    /// <returns>The entity type.</returns>
    protected abstract string GetEntityType();

    /// <summary>
    ///     Converts a domain model to a data model.
    /// </summary>
    /// <param name="domain">The domain model to convert.</param>
    /// <returns>The converted data model.</returns>
    public virtual TDataModel ToDataModel(TDomain domain)
    {
        if (domain == null) return null;

        var model = SimpleMapper.Map<TDomain, TDataModel>(domain);

        CustomDataModelMap(model, domain);

        return model;
    }

    /// <summary>
    ///     Converts a list of domain models to a list of data models.
    /// </summary>
    /// <param name="domains">The list of domain models to convert.</param>
    /// <returns>The converted list of data models.</returns>
    public IList<TDataModel> ToDataModel(IList<TDomain> domains)
    {
        return domains?.Select(ToDataModel).ToList();
    }

    /// <summary>
    ///     Converts a data model to a domain model.
    /// </summary>
    /// <param name="dataModel">The data model to convert.</param>
    /// <returns>The converted domain model.</returns>
    public virtual TDomain ToDomain(TDataModel dataModel)
    {
        if (dataModel == null) return null;

        var tDomain = SimpleMapper.Map<TDataModel, TDomain>(dataModel);

        CustomDomainMap(tDomain, dataModel);

        return tDomain;
    }

    /// <summary>
    ///     Converts a list of data models to a list of domain models.
    /// </summary>
    /// <param name="dataModels">The list of data models to convert.</param>
    /// <returns>The converted list of domain models.</returns>
    public IList<TDomain> ToDomain(IList<TDataModel> dataModels)
    {
        return dataModels?.Select(ToDomain).ToList();
    }

    /// <summary>
    ///     Custom mapping logic from domain model to data model.
    /// </summary>
    /// <param name="dataModel">The data model.</param>
    /// <param name="domain">The domain model.</param>
    protected abstract void CustomDataModelMap(TDataModel dataModel, TDomain domain);

    /// <summary>
    ///     Custom mapping logic from data model to domain model.
    /// </summary>
    /// <param name="tDomain">The domain model.</param>
    /// <param name="dataModel">The data model.</param>
    protected abstract void CustomDomainMap(TDomain tDomain, TDataModel dataModel);
}