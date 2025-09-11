using BookStore.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace BookStore.Api.Helpers
{
    public class CacheResponse : Attribute, IAsyncActionFilter
    {
        private readonly int timeInSecound;

        public CacheResponse(int timeInSecound)
        {
            this.timeInSecound = timeInSecound;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = GetKey(context.HttpContext.Request);
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<ICachedService>();
            var response = await cachedService.GetCashedResponse(key);
            if (!string.IsNullOrEmpty(response))
            {
                var contentResult = new ContentResult
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                context.Result = contentResult;
                return;
            }
            var executingEndpoint = await next();
            if(executingEndpoint.Result is OkObjectResult okResult )
            {
                await cachedService.CacheResponse(key, okResult.Value, TimeSpan.FromSeconds(timeInSecound));
            }
        }

        private string GetKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);
            foreach(var (k,v) in request.Query.OrderBy(Q => Q.Key))
            {
                keyBuilder.Append($"|{k}-{v}");
            }
            return keyBuilder.ToString();
        }
    }
}
