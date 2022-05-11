// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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