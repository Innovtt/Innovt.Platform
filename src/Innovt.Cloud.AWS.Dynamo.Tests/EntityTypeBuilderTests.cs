using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
public class EntityTypeBuilderTests
{   
    [Test]
    public void ConfigureShouldReturnAllMappedAttributes()
    {
        var builder = new EntityTypeBuilder<User>();
        
        var userMap = new UserMap();
        
        userMap.Configure(builder);
        
        Assert.That(builder, Is.Not.Null);
        Assert.That(builder.EntityType, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(builder.EntityType, Is.EqualTo("User"));
            Assert.That(builder.TableName, Is.EqualTo("Users"));
            Assert.That(builder.Pk, Is.EqualTo("PK"));
            Assert.That(builder.Sk, Is.EqualTo("SK"));
        });
        
        var properties = builder.GetProperties();
        
        Assert.That(properties, Is.Not.Null);
        Assert.That(properties, Has.Count.EqualTo(12)); // Because we ignored the Email property

        var emailProperty = builder.GetProperty("Email");
        
        Assert.That(emailProperty, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(emailProperty.MaxLength, Is.EqualTo(50));
            Assert.That(emailProperty.Required, Is.True);
        });
    } 
}