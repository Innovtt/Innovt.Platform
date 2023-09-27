using System;

namespace Innovt.HttpClient.Core
{
    public class ApiException<T>:Exception
    {
        public T ErrorResponse { get; set; }

        public ApiException(T errorResponse)
        {
            ErrorResponse = errorResponse;
        }
    }
}
