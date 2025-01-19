using System.Diagnostics;
using System.Text.Json;
using FluentValidation;
using Serilog;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private const string CONTENT_TYPE = "application/json";

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiFlowException afe)
        {
            context.Response.StatusCode = afe.ResponseStatusCode;
            context.Response.ContentType = CONTENT_TYPE;
            string response = JsonSerializer.Serialize(new { messages = new[] { new { error = afe.Message } } });
            await context.Response.WriteAsync(response);
            LogException(afe);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = CONTENT_TYPE;

            var errorDetails = new
            {
                messages = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };

            string response = JsonSerializer.Serialize(errorDetails);
            await context.Response.WriteAsync(response);
            LogException(ex);

        }
        catch (Exception ex)
        {

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = CONTENT_TYPE;

            string response = JsonSerializer.Serialize(new
            {
                error = "An unexpected error occurred.",
                details = ex.Message
            });

            await context.Response.WriteAsync(response);
            LogException(ex);
        }
    }

    private void LogException(Exception ex)
    {
        Log.ForContext("StackTrace", ex.StackTrace).Error(ex, $"Exception: {ex.Message}");
    }
}
