namespace Innovt.AspNetCore.Model
{
    public class DefaultApiDocumentation
    {   
        public string ApiTitle { get; set; }
        public string ApiDescription { get; set; }
        public string ApiVersion { get; set; }

        public DefaultApiDocumentation(string apiTitle, string apiDescription,
            string apiVersion)
        {
       
            ApiTitle = apiTitle;
            ApiDescription = apiDescription;
            ApiVersion = apiVersion;
        }
    }
}