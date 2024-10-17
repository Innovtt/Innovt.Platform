using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping.Contacts;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
public class DiscriminatorBuilderTests
{   
    [Test]
    public void DiscriminatorShouldReturnNullWhenNotDefined()
    {
        var builder = new EntityTypeBuilder<DynamoContact>();
        
        builder.AutoMap();
        
        Assert.That(builder.Discriminator, Is.Null);
    }
    
    [Test]
    public void DiscriminatorReturnInstanceWhenTypeValueIsPassed()
    {
        var builder = new EntityTypeBuilder<DynamoContact>();
        
        builder.AutoMap();
        
        builder.HasDiscriminator<int>("Type").HasValue<DynamoPhoneContact>(1)
            .HasValue<DynamoEmailContact>(2);

        var discriminator = builder.Discriminator;
            
        Assert.That(discriminator, Is.Not.Null);

        var phone = discriminator.GetValue(1);
        var email = discriminator.GetValue(2);

        Assert.Multiple(() =>
        {
            Assert.That(phone, Is.Not.Null);
            Assert.That(phone, Is.InstanceOf<DynamoPhoneContact>());
            Assert.That(email, Is.Not.Null);
            Assert.That(email, Is.InstanceOf<DynamoEmailContact>());
        });
    }
    
    [Test]
    public void DiscriminatorReturnInstanceWhenInstanceIsPassed()
    {
        var builder = new EntityTypeBuilder<DynamoContact>();
        
        builder.AutoMap();
        
        builder.HasDiscriminator<int>("Type").HasValue(new DynamoPhoneContact()
            {
                CountryCode = "+55"
            }, 1)
            .HasValue(new DynamoEmailContact()
            {
                Value = "michelmob@gmail.com"
            }, 2);

        var discriminator = builder.Discriminator;
            
        Assert.That(discriminator, Is.Not.Null);

        var phone = discriminator.GetValue<DynamoPhoneContact>(1);
        var email = discriminator.GetValue<DynamoEmailContact>(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(phone, Is.Not.Null);
            Assert.That(phone.CountryCode, Is.EqualTo("+55"));
            Assert.That(email, Is.Not.Null);
            Assert.That(email.Value, Is.EqualTo("michelmob@gmail.com"));
        });
    }
    
}