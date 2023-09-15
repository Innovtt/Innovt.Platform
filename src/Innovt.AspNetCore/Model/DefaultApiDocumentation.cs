// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.Model;

public class DefaultApiDocumentation
{
    public DefaultApiDocumentation(string apiTitle, string apiDescription, string apiVersion, string? contactName=null, string? contactEmail=null)
    {
        ApiTitle = apiTitle;
        ApiDescription = apiDescription;
        ApiVersion = apiVersion;
        ContactName = contactName;
        ContactEmail = contactEmail;
    }

    public string ApiTitle { get; set; }
    public string ApiDescription { get; set; }
    public string ApiVersion { get; set; }
    
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    
    
}