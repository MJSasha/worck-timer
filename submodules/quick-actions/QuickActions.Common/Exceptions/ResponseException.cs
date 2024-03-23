using System.Net;

namespace QuickActions.Common.Exceptions
{
    public class ResponseException : Exception
    {
        public ResponseException(int statusCode, object value = null)
        {
            (StatusCode, Value) = (statusCode, value);
        }

        public ResponseException(HttpStatusCode statusCode, object value = null)
        {
            (StatusCode, Value) = ((int)statusCode, value);
        }

        public int StatusCode { get; }

        public object Value { get; }
    }
}