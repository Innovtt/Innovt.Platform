using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
public class EntityTypeBuilderTests
{
    [Test]
    public void CheckMaxLength()
    {
        var builder = new EntityTypeBuilder<User>();
        
        var userMap = new UserMap();

        userMap.Configure(builder);

        builder.Property(p => p.Email).WithMaxLength(50).IsRequired();

        var emailProperty = builder.GetProperty("Email");

        Assert.That(emailProperty, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(emailProperty.MaxLength, Is.EqualTo(50));
            Assert.That(emailProperty.Required, Is.True);
        });
    }
    
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
        Assert.That(properties, Has.Count.EqualTo(13));

        var emailProperty = builder.GetProperty("Email");

        Assert.That(emailProperty, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(emailProperty.MaxLength, Is.EqualTo(50));
            Assert.That(emailProperty.Required, Is.True);
        });
    }
    [Test]
    public void IgnoringProperties()
    {
        var builder = new EntityTypeBuilder<User>();

        builder.AutoMap();
        
        var properties = builder.GetProperties();//Should have 13 properties
        
        Assert.That(properties, Is.Not.Null);
        Assert.That(properties, Has.Count.EqualTo(13)); // Because we ignored the Email property
        
        //Adding the same property should be ignored
        builder.Ignore(u => u.FirstName);//Should have 12 properties
        
        properties = builder.GetProperties();//Should have 12 properties

        Assert.That(properties, Is.Not.Null);
        Assert.That(properties, Has.Count.EqualTo(12)); // Because we ignored the Email property

        builder.Ignore("NameAndDate");//Ignoring a property that does not exist should be ignored
        
        properties = builder.GetProperties();//Should have 12 properties

        Assert.That(properties, Is.Not.Null);
        Assert.That(properties, Has.Count.EqualTo(12)); // Because we ignored the Email property
    }
    
    [Test]
    public void AddTheSamePropertyShouldBeIgnored()
    {
        var builder = new EntityTypeBuilder<User>(); // 13 properties from UserMap

        var userMap = new UserMap(); 

        userMap.Configure(builder);
        
        //Adding the same property should be ignored
        builder.Property(u => u.FirstName);
        
        var properties = builder.GetProperties();
        
        Assert.That(properties, Is.Not.Null);
        Assert.That(properties, Has.Count.EqualTo(13)); // Because we ignored the Email property
    }
}