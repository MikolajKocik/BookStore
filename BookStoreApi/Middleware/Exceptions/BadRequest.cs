namespace BookStoreApi.Middleware.Exceptions
{
    public class BadRequest : Exception
    {
        public BadRequest() : base("Bad request") { }
        public BadRequest(string message) : base(message) { }
        public BadRequest(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
