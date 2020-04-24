using System;
using System.Diagnostics;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Core.Utilities
{
    public class StopWatchHelper:IDisposable
    {
        private readonly ILogger logger;
        private readonly string action;
        private Stopwatch stopwatch = null;
        
        public StopWatchHelper(ILogger logger,string action)
        {
            this.logger = logger;
            this.action = action;
            stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            try
            {
				logger.Info($"Action={action},ElapsedMilliseconds={ stopwatch.ElapsedMilliseconds}");
                stopwatch = null;
            }
            catch {}


        }
    }
}