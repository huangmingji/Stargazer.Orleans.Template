using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stargazer.Orleans.Template.Domain.Share.Resources;
using Stargazer.Orleans.Template.Host.Resources;

namespace Stargazer.Orleans.Template.Host.Middlewares;

public class GlobalExceptionFilter(IHostEnvironment env, LocalizationService localization) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;
        var code = GetCode(ex);
        var httpStatusCode = GetHttpStatusCode(ex);
        var language = localization.GetCurrentLanguage(context.HttpContext);
        var message = localization.GetMessage(code, language);

        var response = new ErrorResponse
        {
            Code = code,
            Message = message
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = httpStatusCode,
            DeclaredType = typeof(ErrorResponse)
        };
        context.ExceptionHandled = true;
    }

    private static string GetCode(Exception ex)
    {
        if (ex is LocalizedException localized)
        {
            return localized.Code;
        }
        return ex.Message;
    }

    private static int GetHttpStatusCode(Exception ex)
    {
        if (ex is LocalizedException localized)
        {
            return localized.HttpStatusCode;
        }
        return ex switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            InvalidOperationException => 403,
            KeyNotFoundException => 404,
            InvalidCastException => 409,
            _ => 500
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