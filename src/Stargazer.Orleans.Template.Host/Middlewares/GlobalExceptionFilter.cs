using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Stargazer.Orleans.Template.Host.Middlewares;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public GlobalExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var response = HandleUnknownException(context.Exception);

        context.Result = new ObjectResult(response)
        {
            StatusCode = response.Code,
            DeclaredType = typeof(ErrorResponse)
        };
        context.ExceptionHandled = true;
    }

    private ErrorResponse HandleUnknownException(Exception ex)
    {
        var code = ex switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            InvalidOperationException => 403,
            KeyNotFoundException => 404,
            InvalidCastException => 409,
            _ => 500
        };

        return new ErrorResponse
        {
            Code = code,
            Message = code == 500 && !_env.IsDevelopment() ? "服务器内部错误" : ex.Message
        };
    }
}

public static class GlobalExceptionFilterExtensions
{
    public static IMvcBuilder AddGlobalExceptionFilter(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options => options.Filters.Add<GlobalExceptionFilter>());
        return builder;
    }
}