namespace Innovt.AspNetCore.Model
{
    public class DefaultApiDocumentation
    {
        public bool EnableDocInProduction { get; set; }
        public string ApiTitle { get; set; }
        public string ApiDescription { get; set; }
        public string ApiVersion { get; set; }

        public DefaultApiDocumentation(bool enableDocInProduction, string apiTitle, string apiDescription,
            string apiVersion)
        {
            EnableDocInProduction = enableDocInProduction;
            ApiTitle = apiTitle;
            ApiDescription = apiDescription;
            ApiVersion = apiVersion;
        }
    }
}