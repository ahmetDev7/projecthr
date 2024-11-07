using System.Text.Json;
using FluentValidation;

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
        catch(ApiFlowException afe){
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            context.Response.ContentType = CONTENT_TYPE;
            var response = JsonSerializer.Serialize(new { error = afe.Message });
            await context.Response.WriteAsync(response);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = CONTENT_TYPE;

            var errorDetails = new
            {
                messages = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };

            var response = JsonSerializer.Serialize(errorDetails);
            await context.Response.WriteAsync(response);
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = CONTENT_TYPE;
            var response = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
            await context.Response.WriteAsync(response);
        }
    }
}
