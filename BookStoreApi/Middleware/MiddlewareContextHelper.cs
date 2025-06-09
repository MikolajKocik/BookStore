namespace BookStoreApi.Middleware
{
    public static class MiddlewareContextHelper
    {
        public static async Task<HttpContext> Response(
            this HttpContext context,
            int statusCode,
            int statusCodeNumber,
            string title,
            Exception ex
            )
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var problemDetails = new 
            { 

                Type = $"https://httpsstatuses/{statusCodeNumber}",
                Title = title,
                Status = statusCode,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);

            return context;
        }
    }
}
