using EmployeeManagement.Domain.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EmployeeManagement.Web.Pages.Employees;

internal static class EmployeeModelStateExtensions
{
    public static void AddEmployeeErrors(
        this ModelStateDictionary modelState,
        IEnumerable<AppError> errors)
    {
        foreach (var error in errors)
        {
            var key = error.Code switch
            {
                "Employee.Email.Exists" => "Form.Email",
                "Employee.MobileNumber.Exists" => "Form.MobileNumber",
                "Employee.Department.NotFound" => "Form.DepartmentId",
                _ => "Form" // Form-level errors
            };

            modelState.AddModelError(key, error.Description);
        }
    }
}
