namespace Innovt.Core.Exceptions
{
    /// <summary>
    /// You can use it to create custom error messages that will be used by our framework.
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="message">The message that you want to send.</param>
        public ErrorMessage(string message)
        {
            this.Message = message;
        }

        public ErrorMessage()
        {
            
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="message">The message that you want to send.</param>
        /// <param name="propertyName">The property(optional) that this error happened.</param>
        public ErrorMessage(string message, string propertyName)
        {
            this.Message = message;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="message">The message that you want to send.</param>
        /// <param name="propertyName">The property(optional) that this error happened.</param>
        /// <param name="code">The code of you error</param>
        public ErrorMessage(string message, string propertyName,string code)
        {
            this.Message = message;
            this.PropertyName = propertyName;
            this.Code = code;
        }


        public string Code { get; protected set; }
        public string Message { get; protected set; }
        public string PropertyName { get; protected set; }
    }
}
