using System.Net;
using Domain.Models;
using Infrastructure.Exceptions;
using Newtonsoft.Json;

namespace Web.Middlewares;

internal class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = string.Empty;
        int statusCode;
        string contentType;
        
        switch (exception)
        {
            case BadRequestJsonException badRequestJsonException:
                contentType = "application/json";
                statusCode = badRequestJsonException.StatusCode;
                response = JsonConvert.SerializeObject(new ErrorResponse(exception.Message));
                break;
            case BadHttpRequestException badHttpRequestException:
                contentType = "text/html";
                statusCode = badHttpRequestException.StatusCode;
                break;
            case InternalServerJsonException:
                contentType = "application/json";
                statusCode = (int)HttpStatusCode.InternalServerError;
                response = JsonConvert.SerializeObject(new ErrorResponse(exception.Message));
                break;
            default:
                contentType = "text/html";
                statusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        
        context.Response.ContentType = contentType;
        context.Response.StatusCode = statusCode;
        
        return context.Response.WriteAsync(response);
    }
}