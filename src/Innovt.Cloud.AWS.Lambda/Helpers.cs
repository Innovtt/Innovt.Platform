// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.IO;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;

namespace Innovt.Cloud.AWS.Lambda
{
    public static class Helpers
    {
        public static string GetEnvironmentVariable(string varName, string defaultValue, ILambdaContext context = null)
        {
            var variable = Environment.GetEnvironmentVariable(varName);

            if (variable == null && context?.ClientContext?.Environment != null)
                if (context.ClientContext.Environment.ContainsKey(varName))
                    variable = context.ClientContext.Environment[varName];

            return variable ?? defaultValue;
        }


        public static IConfigurationRoot BuildConfiguration(string environmentVariableName, string defaultValue,
            ILambdaContext context = null)
        {
            var environmentVariable = GetEnvironmentVariable(environmentVariableName, defaultValue, context);

            var configurationName = $"appsettings.{environmentVariable}.json";

            return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configurationName, true, true).Build();
        }
    }
}