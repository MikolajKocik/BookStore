using BookStoreApi.Middleware.Exceptions;

namespace BookStoreApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequest ex)
            {
                await MiddlewareContextHelper.Response(
                   context,
                   StatusCodes.Status400BadRequest,
                   400,
                   "Bad request",
                   ex);
            }
            catch (ArgumentNullException ex)
            {
                await MiddlewareContextHelper.Response(
                    context,
                    StatusCodes.Status404NotFound,
                    404,
                    "Not found",
                    ex);
            }
            catch (Exception ex)
            {
               await MiddlewareContextHelper.Response(
                    context,
                    StatusCodes.Status500InternalServerError,
                    500,
                    "Internal server error",
                    ex);
            }
        }
    }
}
