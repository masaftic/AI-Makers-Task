using EmployeeManagement.Application.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Api.Pages.Employees;

public sealed class EditModel(EmployeeService employeeService) : PageModel
{
    [BindProperty]
    public EmployeeFormModel Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetByIdAsync(id, cancellationToken);
        if (employee is null)
        {
            return NotFound();
        }

        Form = EmployeeFormModel.FromDto(employee);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        Form.IsEditMode = true;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await employeeService.UpdateAsync(id, Form.ToDto(), cancellationToken);
        if (result.IsError)
        {
            if (result.FirstError.Type == Domain.Shared.ErrorType.NotFound)
            {
                return NotFound();
            }

            ModelState.AddEmployeeErrors(result.Errors);
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
