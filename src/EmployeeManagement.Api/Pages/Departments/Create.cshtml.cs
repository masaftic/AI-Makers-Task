using EmployeeManagement.Application.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Api.Pages.Departments;

public sealed class CreateModel(IDepartmentService departmentService) : PageModel
{
    [BindProperty]
    public DepartmentFormModel Form { get; set; } = new();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        Form.IsEditMode = false;
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await departmentService.CreateAsync(Form.ToDto(), cancellationToken);
        if (result.IsError)
        {
            ModelState.AddDepartmentErrors(result.Errors);
            return Page();
        }

        return RedirectToPage("/Departments/Index");
    }
}
