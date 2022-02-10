// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.CrossCutting.Log;
using System;
using System.Diagnostics;

namespace Innovt.Core.Utilities
{
    public class StopWatchHelper : IDisposable
    {
        private readonly string action;
        private readonly ILogger logger;
        private Stopwatch stopwatch;

        public StopWatchHelper(ILogger logger, string action)
        {
            this.logger = logger;
            this.action = action;
            stopwatch = Stopwatch.StartNew();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        ~StopWatchHelper()
        {
            Dispose(false);
        }
    }
}