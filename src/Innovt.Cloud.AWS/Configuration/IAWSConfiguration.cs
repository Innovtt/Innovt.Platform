namespace Innovt.Cloud.AWS.Configuration
{
    public interface IAWSConfiguration: IConfiguration
    {
        string AccountNumber { get; set; }
    }
}
