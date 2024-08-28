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
        builder.AutoMap().HasSortKey("PK").WithTableName("Users");
            .WithOneTableHashKey()
            .WithOneTableRangeKey().WithTableName("Users");

        //O mapeamento da volta nao existe, o que existe é o mapeamento na ida para o banco. Exemplo
        // Quando estamos mandando para p banco nos definiremos quais campos serao utilizados como pk e sk.
        //Tambem podemos setar em um campo o prefixo utilizado e o separador de chave.
        //Exemplo, se o id do usuario for 123456, o prefixo for USER# e o separador for #, a chave que sera utilizada no banco sera USER#123456
        // para isso nos iremos adicionar um map no builder para uma propriedade dizendo que o sistema deve chamar aquele action para setar o valor doquela coluna 
        // no banco.
        // No retorno, o sistema vai trazer a coluna que esta no banco mas tb deve trazer os campos que foram responsáveis por montar a chave e sempre que a chave for
        // montada ele podera ser reconstruida. 
        // lembrando que nao podemos ter uma chave virtual que nao podera ser setada no objeto sobre a condicao de que ela nao exista no banco.
        
        
        
        builder.HasHashKeyPrefix("USER#");
    }
}