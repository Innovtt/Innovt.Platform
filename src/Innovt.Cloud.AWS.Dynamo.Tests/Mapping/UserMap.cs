using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;


namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Implementation of IEntityTypeDataModelMapper for mapping the UserSample entity to its corresponding data model.
/// </summary>
public class UserMap : IEntityTypeDataModelMapper<User>
{
    /// <summary>
    ///     Configures the mapping between the UserSample entity and its corresponding data model using the provided
    ///     EntityTypeBuilder.
    /// </summary>
    /// <param name="builder">The EntityTypeBuilder used to configure the mapping.</param>
    public void Configure([NotNull] EntityTypeBuilder<User> builder)
    {
        builder.AutoMap().WithTableName("Users", "#");

        //quero que o valor dessa propriedade vai ser o resultado de uma função
        
        //Map é utilizado para definir como uma propriedade será mapeada
        builder.WithOneTableHashKey().SetDynamicValueDelegate(u =>
        {
            return "USER#"+u.Id;
        });

        builder.WithOneTableRangeKey().WithValue("PROFILE");
        builder.Property(u => u.Email).WithMaxLength(50).IsRequired();
        
        
        
        //Como vou setar aqui a propriedade Id como chave primária, não preciso setar o nome da propriedade
        
        builder.HasHashKeyPrefix("USER");
    }
}