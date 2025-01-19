using System.Text;
using Serilog;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        // Log the request body only on (POST/PUT)
        if ((request.Method == HttpMethods.Post || request.Method == HttpMethods.Put) && request.ContentLength > 0)
        {
            var requestBody = await GetRequestBody(request);
            Log.ForContext("Body", requestBody)
                .ForContext("Method", request.Method)
                .ForContext("Route", request.Path)
                .Information($"REQUEST: {request.Method} {request.Path}");
            request.Body.Position = 0;
        }
        else
        {
            _logger.LogInformation($"REQUEST: {request.Method} {request.Path}");
        }

        var originalBodyStream = context.Response.Body;
        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await _next(context);

            string? responseBody = await GetResponseBody(memoryStream);
            Log.ForContext("Body", responseBody)
                .ForContext("Route", request.Path)
                .Information($"RESPONSE: {request.Method} {request.Path} StatusCode: {context.Response.StatusCode}");

            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string?> GetRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        string requestContent = Encoding.UTF8.GetString(buffer);
        request.Body.Position = 0;
        return requestContent;
    }

    private async Task<string?> GetResponseBody(MemoryStream memoryStream)
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseContent = await new StreamReader(memoryStream).ReadToEndAsync();
        memoryStream.Seek(0, SeekOrigin.Begin);
        return responseContent;
    }
}
