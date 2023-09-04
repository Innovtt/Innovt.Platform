using System.Linq;
using Innovt.Core.Exceptions;
using Innovt.Core.Validation;
using Innovt.Domain.Contacts;
using NUnit.Framework;

namespace Innovt.Domain.Tests;

[TestFixture]
public class ContactTests
{
    [Test]
    public void ContactValidate_Should_Return_Error_When_ContactType_Is_Null()
    {       
        var contact = new Contact()
        {
            Name = "Innovt",
            Description = "Phone Test",
     
            Value = "71991433757",
        };

        var error = Assert.Throws<BusinessException>(()=>contact.EnsureIsValid());
        
        Assert.That(error, Is.Not.Null);
    }
    
    [Test]
    public void ContactValidate_Should_Validate_IfContactTypeHasNoValidation()
    {
        var contactType = ContactType.Create("Phone", "The phone number");
            
        var contact = new Contact()
        {
            Name = "Innovt",
            Description = "Phone Test",
            Type = contactType,
            Value = "71991433757",
        };

        contact.EnsureIsValid();
        
        Assert.Pass();
    }
    
    [Test]
    public void ContactValidate_Should_ThrowException_When_TypeValidation_DoesNotMatch()
    {
        var contactType = ContactType.Create("Email", "The email");
        contactType.RegexValidation = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        
        var contact = new Contact()
        {
            Name = "Innovt",
            Description = "Phone Test",
            Type = contactType,
            Value = "71991433757",
        };

        var error = Assert.Throws<BusinessException>(()=>contact.EnsureIsValid());
        
        Assert.That(error, Is.Not.Null);
        
        Assert.That(error.Errors.First().Message, Is.EqualTo("The value 71991433757 is not valid for Email."));
      
    }
  
    
    
    
    
    
}