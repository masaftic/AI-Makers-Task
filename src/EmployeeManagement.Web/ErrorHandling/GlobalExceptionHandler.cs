using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Web.ErrorHandling;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException => (
                StatusCodes.Status400BadRequest,
                "Validation error",
                exception.Message),
            DbUpdateConcurrencyException => (
                StatusCodes.Status409Conflict,
                "Concurrency conflict",
                "The data changed before the request could be completed."),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Unexpected error",
                "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = statusCode;

        if (AcceptsHtml(httpContext.Request))
        {
            var errorPath = QueryString.Create(
                [
                    new KeyValuePair<string, string?>("statusCode", statusCode.ToString()),
                    new KeyValuePair<string, string?>("title", title),
                    new KeyValuePair<string, string?>("detail", detail),
                    new KeyValuePair<string, string?>("traceId", httpContext.TraceIdentifier)
                ]);

            httpContext.Response.Redirect($"/Error{errorPath}");
            return true;
        }

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path,
                Extensions =
                {
                    ["traceId"] = httpContext.TraceIdentifier
                }
            },
            cancellationToken);

        return true;
    }

    private static bool AcceptsHtml(HttpRequest request)
        => request.GetTypedHeaders().Accept?.Any(mediaType =>
            mediaType.MediaType.HasValue &&
            mediaType.MediaType.Value.Equals(
                "text/html",
                StringComparison.OrdinalIgnoreCase)) == true;
}
