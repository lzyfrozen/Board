using System.Net;
using Board.Infrastructure.Models;
using Board.ToolKits;
using Board.Web.Models;

namespace Board.Web.Middleware
{
    /// <summary>
    /// 异常处理中间件
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandlerAsync(context, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                if (statusCode != StatusCodes.Status200OK)
                {
                    Enum.TryParse(typeof(HttpStatusCode), statusCode.ToString(), out object? message);
                    await ExceptionHandlerAsync(context, message?.ToString());
                }
            }
        }

        /// <summary>
        /// 异常处理，返回JSON
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ExceptionHandlerAsync(HttpContext context, string? message)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = StatusCodes.Status200OK;

            var result = new ApiResult<string>()
            {
                Result = false,
                Message = message
            };


            await context.Response.WriteAsync(result.ToJson());
            // 错误日志记录
            _logger.LogError($"{context.Request.Path}|{message}");
        }
    }

}
