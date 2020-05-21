using System;

namespace Innovt.Core.CrossCutting.Log
{
    public interface ILogger
    {
        /// <summary>
        /// Debug is the noisiest level, rarely (if ever) enabled for a production app.
        /// </summary>
        /// <param name="message">The message will follow the Serilog Pattern.</param>
        void Debug(string message);

        /// <summary>
        /// Debug is the noisiest level, rarely (if ever) enabled for a production app.
        /// </summary>
        /// <param name="messageTemplate">The message templete will follow the Serilog Pattern.</param>
        /// <param name="propertyValues">Properties that will be used as template of the message</param>
        void Debug(string messageTemplate, params object[] propertyValues);

        /// <summary>
        /// Debug is the noisiest level, rarely (if ever) enabled for a production app.
        /// </summary>
        /// <param name="exception">An Exception parameter</param>
        /// <param name="messageTemplate">The message templete will follow the Serilog Pattern.</param>
        void Debug(Exception exception, string messageTemplate);

        /// <summary>
        /// Debug is the noisiest level, rarely (if ever) enabled for a production app.
        /// </summary>
        /// <param name="exception">An exception</param>
        /// <param name="messageTemplate">The message templete will follow the Serilog Pattern.</param>
        /// <param name="propertyValues"></param>
        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);


        /// <summary>
        /// An error logger
        /// </summary>
        /// <param name="message">The message templete will follow the Serilog Pattern.</param>
        void Error(string message);
       
        void Error(string messageTemplate, params object[] propertyValues);
      
        void Error(Exception exception, string messageTemplate);
       
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);

        void Fatal(string message);

        void Fatal(string messageTemplate, params object[] propertyValues);

        void Fatal(Exception exception, string messageTemplate);

        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

        void Info(string message);

        void Info(string messageTemplate, params object[] propertyValues);

        void Info(Exception exception, string messageTemplate);


        void Info(Exception exception, string messageTemplate, params object[] propertyValues);
        
        void Verbose(string message);

        void Verbose(string messageTemplate, params object[] propertyValues);

        void Verbose(Exception exception, string messageTemplate);

        void Verbose(Exception exception, string messageTemplate, params object[] propertyValues);

        void Warning(string message);

        void Warning(string messageTemplate, params object[] propertyValues);

        void Warning(Exception exception, string messageTemplate);

        void Warning(Exception exception, string messageTemplate, params object[] propertyValues);
    }

    public interface ILogger<T>:ILogger
    {

    }
}

    
     