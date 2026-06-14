using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Domain.Shared;

namespace EmployeeManagement.Domain.EmployeeRoot;

public sealed record MobileNumber
{
    public string Value { get; }

    private MobileNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Validates and normalizes a mobile number
    /// </summary>
    public static Result<MobileNumber> TryCreate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return AppError.Validation(
                "MobileNumber.Required",
                "Mobile number is required.");
        }

        var normalized = value.Trim().Replace(" ", "").Replace("-", "");

        if (!normalized.StartsWith('+') ||
            normalized.Length is < 8 or > 16 ||
            normalized[1..].Any(character => !char.IsAsciiDigit(character)))
        {
            return AppError.Validation(
                "MobileNumber.Invalid",
                "Mobile number must use international format, such as +9999999999.");
        }

        return new MobileNumber(normalized);
    }

    public static MobileNumber Create(string? value)
        => TryCreate(value).Match(
            success => success,
            errors => throw new ValidationException(errors.First().Description));

    public override string ToString() => Value;
}