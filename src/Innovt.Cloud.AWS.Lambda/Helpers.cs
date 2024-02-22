// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System;
using System.IO;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;

namespace Innovt.Cloud.AWS.Lambda;

/// <summary>
///     A utility class containing helper methods for common tasks in AWS Lambda functions.
/// </summary>
public static class Helpers
{
    /// <summary>
    ///     Retrieves the value of an environment variable or a default value.
    /// </summary>
    /// <param name="varName">The name of the environment variable to retrieve.</param>
    /// <param name="defaultValue">The default value to use if the environment variable is not found.</param>
    /// <param name="context">The Lambda context associated with the function execution (optional).</param>
    /// <returns>The value of the environment variable if found, or the default value if not found.</returns>
    public static string GetEnvironmentVariable(string varName, string defaultValue, ILambdaContext context = null)
    {
        var variable = Environment.GetEnvironmentVariable(varName);

        if (variable != null || context?.ClientContext?.Environment == null) return variable ?? defaultValue;


        if (context.ClientContext.Environment.ContainsKey(varName))
            variable = context.ClientContext.Environment[varName];

        return variable ?? defaultValue;
    }

    /// <summary>
    ///     Builds a configuration object based on an environment variable and default value.
    /// </summary>
    /// <param name="environmentVariableName">The name of the environment variable containing the configuration name.</param>
    /// <param name="defaultValue">The default configuration name to use if the environment variable is not found.</param>
    /// <param name="context">The Lambda context associated with the function execution (optional).</param>
    /// <returns>An <see cref="IConfigurationRoot" /> object representing the configuration.</returns>
    public static IConfigurationRoot BuildConfiguration(string environmentVariableName, string defaultValue,
        ILambdaContext context = null)
    {
        var environmentVariable = GetEnvironmentVariable(environmentVariableName, defaultValue, context);

        var configurationName = $"appsettings.{environmentVariable}.json";

        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationName, true, true).Build();
    }
}