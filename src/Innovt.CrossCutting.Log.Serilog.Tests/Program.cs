// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.Log.Serilog.Tests

using System.Diagnostics;

namespace Innovt.CrossCutting.Log.Serilog.Tests;

/// <summary>
///     Represents the entry point and test methods for the application.
/// </summary>
public class Program
{
    /// <summary>
    ///     Runs a simple test without any enricher.
    /// </summary>
    public static void SimpleTestWithoutEnricher()
    {
        using var ac = new Activity("sample");
        ac.Start();

        var logger = new Logger();

        logger.Info("Teste");

        try
        {
            throw new Exception("exception not handled");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error");
        }

        logger.Error("Teste", "4564");
        //
        ac.Stop();
    }

    /// <summary>
    ///     Runs a simple test with DataDog enricher.
    /// </summary>
    public static void SimpleTestWithDataDogEnricher()
    {
        using var ac = new Activity("sample");
        ac.Start();

        var logger = new Logger(new DataDogEnrich());

        logger.Info("Teste");

        try
        {
            throw new Exception("exception not handled");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error");
        }

        logger.Error("Teste", "4564");
        //
        ac.Stop();
    }

    /// <summary>
    ///     The main entry point of the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
        //SimpleTestWithoutEnricher();
        SimpleTestWithDataDogEnricher();
    }
}