public class ApiFlowException : Exception
{
    // Microsoft.AspNetCore.Http -> StatusCodes

    public int ResponseStatusCode { get; set; }
    public ApiFlowException(string message, int? responseStatusCode = null) : base(message)
    {
        ResponseStatusCode = (int)(responseStatusCode != null ? responseStatusCode : StatusCodes.Status422UnprocessableEntity);
    }
}
