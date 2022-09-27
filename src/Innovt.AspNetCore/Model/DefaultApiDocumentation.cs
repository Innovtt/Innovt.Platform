// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.Model;

public class DefaultApiDocumentation
{
    public DefaultApiDocumentation(string apiTitle, string apiDescription,
        string apiVersion)
    {
        ApiTitle = apiTitle;
        ApiDescription = apiDescription;
        ApiVersion = apiVersion;
    }

    public string ApiTitle { get; set; }
    public string ApiDescription { get; set; }
    public string ApiVersion { get; set; }
}