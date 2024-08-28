using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Implementation of IEntityTypeDataModelMapper for mapping the UserSample entity to its corresponding data model.
/// </summary>
public class CompanyMap : IEntityTypeDataModelMapper<Company>
{
    /// <summary>
    ///     Configures the mapping between the UserSample entity and its corresponding data model using the provided
    ///     EntityTypeBuilder.
    /// </summary>
    /// <param name="builder">The EntityTypeBuilder used to configure the mapping.</param>
    public void Configure([NotNull] EntityTypeBuilder<Company> builder)
    {
        builder.AutoMap().WithOneTableHashKey().WithOneTableRangeKey().WithTableName(nameof(Company));

        builder.Property(p => p.Name).AsDecimal().HasColumnName("Name2");

        builder.WithTableName(nameof(Company));
    }
}