using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovt.CrossCutting.Log.Serilog.Tests
{
    public class Program
    {
        public static void Main(string[] args) {


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
                logger.Error(ex,"Error");
                throw;
            }
            
            logger.Error("Teste", "4564");
//
           ac.Stop();
        }
    }
}
