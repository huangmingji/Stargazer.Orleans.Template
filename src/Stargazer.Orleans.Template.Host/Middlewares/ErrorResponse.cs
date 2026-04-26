namespace Stargazer.Orleans.Template.Host.Middlewares;

public class ErrorResponse
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
}