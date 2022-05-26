using System.Diagnostics;

namespace Innovt.CrossCutting.Log.Serilog.Tests;

public class Program
{
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


    public static void Main(string[] args)
    {
        //SimpleTestWithoutEnricher();
        SimpleTestWithDataDogEnricher();
    }
}