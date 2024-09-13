using HomeWork.Redis.Domain.Dto;
using HomeWork.Redis.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HomeWork.Redis.WebApp
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new RequestResult
            {
                Message = exception.Message,
                Code = Codes.BadRequest
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }


        private Task HandleExceptionAsync(HttpContext context, BadRequestException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new RequestResult
            {
                Message = exception.Message,
                Code = exception.Code
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
