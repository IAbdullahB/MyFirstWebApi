namespace MyFirstWebAPI.Middlewares
{
    public class RateLimitingMiddleware
    {
        private static int _cnt = 0;


        private readonly RequestDelegate _next;

        private static DateTime _lastRequestTime = DateTime.Now;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _cnt++;

            if(DateTime.Now.Subtract(_lastRequestTime).TotalSeconds > 10)
            //this means that the last request was more than 10 seconds ago, so we reset the counter
            {
                _cnt = 1;
                _lastRequestTime = DateTime.Now;
                await _next(context);
            }
            else
            {
                if(_cnt > 5) // this means that there have been more than 5 requests in the last 10 seconds
                {
                    context.Response.StatusCode = 429; // Too Many Requests
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                }
                else
                {
                    _lastRequestTime = DateTime.Now;
                    await _next(context);
                }
            }
        }
    }
}
