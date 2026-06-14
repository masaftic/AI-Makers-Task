using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeManagement.Api.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DomainMobileNumberAttribute : ValidationAttribute, IClientModelValidator
{
    private const string DefaultMessage =
        "Mobile number must use international format, such as +9999999999.";

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null or "")
        {
            return ValidationResult.Success;
        }

        var result = MobileNumber.TryCreate(value.ToString());

        return result.IsError
            ? new ValidationResult(result.FirstError.Description)
            : ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        Merge(context.Attributes, "data-val", "true");
        Merge(context.Attributes, "data-val-domain-mobile", ErrorMessage ?? DefaultMessage);
    }

    private static void Merge(IDictionary<string, string> attributes, string key, string value)
    {
        if (!attributes.ContainsKey(key))
        {
            attributes.Add(key, value);
        }
    }
}
