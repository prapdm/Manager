using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Manager.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private Stopwatch _stopwatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopwatch = new Stopwatch();
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var elipsedMiliseconds = _stopwatch.ElapsedMilliseconds;
            if (elipsedMiliseconds / 1000 > 4)
            {

                _logger.LogInformation($"Request [{context.Request.Method}] at {context.Request.Path} took {elipsedMiliseconds} ms");
            }

        }


    }
}
