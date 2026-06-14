using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Domain.Shared;

namespace EmployeeManagement.Domain.EmployeeRoot;

public sealed record EmailAddress
{
    public string Value { get; }

    private EmailAddress(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Validates and normalizes an email address
    /// </summary>
    public static Result<EmailAddress> TryCreate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return AppError.Validation(
                "Email.Required",
                "Email is required.");
        }

        var normalized = value.Trim().ToLowerInvariant();

        if (!System.Net.Mail.MailAddress.TryCreate(normalized, out var address) || address.Address != normalized)
        {
            return AppError.Validation(
                "Email.Invalid",
                "Email is invalid.");
        }

        return new EmailAddress(normalized);
    }

    public static EmailAddress Create(string? value)
        => TryCreate(value).Match(
            success => success,
            errors => throw new ValidationException(errors.First().Description));

    public override string ToString() => Value;
}
