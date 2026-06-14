using EmployeeManagement.Application.Departments;
using EmployeeManagement.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Api.Pages.Departments;

public sealed class EditModel(IDepartmentService departmentService) : PageModel
{
    [BindProperty]
    public DepartmentFormModel Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetByIdAsync(id, cancellationToken);
        if (department is null)
        {
            return NotFound();
        }

        Form = DepartmentFormModel.FromDto(department);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Form.IsEditMode = true;
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await departmentService.UpdateAsync(id, Form.ToDto(), cancellationToken);
        if (result.IsError)
        {
            if (result.FirstError.Type == ErrorType.NotFound)
            {
                return NotFound();
            }

            ModelState.AddDepartmentErrors(result.Errors);
            return Page();
        }

        return RedirectToPage("/Departments/Index");
    }
}
