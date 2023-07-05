using Board.Infrastructure.Models;
using Board.ToolKits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Board.Web.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter//IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment hostEnvironment, ILogger<GlobalExceptionFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var result = new ApiResult<string>()
                {
                    Result = false,
                    Message = "服务器发生未处理的异常"
                };

                if (_hostEnvironment.IsDevelopment())
                {
                    result.Message += "," + context.Exception.Message;
                    result.Data = context.Exception.StackTrace;
                }

                context.Result = new ContentResult
                {
                    // 返回状态码设置为200，表示成功
                    StatusCode = StatusCodes.Status200OK,
                    // 设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = result.ToJson()
                };

                _logger.LogError(context.Exception, $"{context.HttpContext.Request.Path}|{context.Exception.Message}");
            }
            context.ExceptionHandled = true;// 设置为true，表示异常已经被处理了
            return Task.CompletedTask;
        }
    }
}

