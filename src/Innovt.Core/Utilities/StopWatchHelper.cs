using System;
using System.Diagnostics;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Core.Utilities
{
    public class StopWatchHelper : IDisposable
    {
        private readonly ILogger logger;
        private readonly string action;
        private Stopwatch stopwatch = null;

        public StopWatchHelper(ILogger logger, string action)
        {
            this.logger = logger;
            this.action = action;
            stopwatch = Stopwatch.StartNew();
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                logger.Info($"Action={action},ElapsedMilliseconds={stopwatch.ElapsedMilliseconds}");
                stopwatch = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e); //todo: colocar no log
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StopWatchHelper()
        {
            Dispose(false);
        }
    }
}