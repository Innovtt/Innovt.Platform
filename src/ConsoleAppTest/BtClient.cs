// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using Innovt.Cloud.AWS.Configuration;

namespace ConsoleAppTest;

public class BtClient
{
    private readonly IAwsConfiguration configuration;

    public BtClient(IAwsConfiguration configuration)
    {
        this.configuration = new AssumeRoleAWSConfiguration(configuration, "", "", "");
    }

    public void SalvarNaBtg()
    {
    }
}