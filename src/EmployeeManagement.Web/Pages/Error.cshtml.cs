using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Web.Pages;

public sealed class ErrorModel : PageModel
{
    public int HttpStatusCode { get; private set; }
    public string Title { get; private set; } = "";
    public string Detail { get; private set; } = "";
    public string? TraceId { get; private set; }

    public void OnGet(
        [FromQuery] int statusCode = StatusCodes.Status500InternalServerError,
        [FromQuery] string? title = null,
        [FromQuery] string? detail = null,
        [FromQuery] string? traceId = null)
    {
        HttpStatusCode = statusCode;
        Title = title ?? "Unexpected error";
        Detail = detail ?? "An unexpected error occurred.";
        TraceId = traceId;

        Response.StatusCode = statusCode;
    }
}
