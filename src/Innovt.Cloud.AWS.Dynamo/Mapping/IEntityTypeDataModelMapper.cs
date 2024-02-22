using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Mapping;

/// <summary>
///     Interface for configuring the mapping between an entity type and its corresponding data model using an
///     EntityTypeBuilder.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IEntityTypeDataModelMapper<T> where T : class
{
    /// <summary>
    ///     Configures the mapping between the entity type and its corresponding data model using the provided
    ///     EntityTypeBuilder.
    /// </summary>
    /// <param name="builder">The EntityTypeBuilder used to configure the mapping.</param>
    public void Configure([NotNull] EntityTypeBuilder<T> builder);
}