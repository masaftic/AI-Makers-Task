using EmployeeManagement.Domain.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EmployeeManagement.Web.Pages.Departments;

internal static class DepartmentModelStateExtensions
{
    public static void AddDepartmentErrors(
        this ModelStateDictionary modelState,
        IEnumerable<AppError> errors)
    {
        foreach (var error in errors)
        {
            var key = error.Code == "Department.Name.Exists" ? "Form.Name" : "Form";
            modelState.AddModelError(key, error.Description);
        }
    }
}
