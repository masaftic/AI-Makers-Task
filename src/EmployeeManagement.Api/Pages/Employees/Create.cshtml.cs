using EmployeeManagement.Application.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Api.Pages.Employees;

public sealed class CreateModel(EmployeeService employeeService) : PageModel
{
    [BindProperty]
    public EmployeeFormModel Form { get; set; } = new();

    public void OnGet()
    {
        Form.HireDate = DateOnly.FromDateTime(DateTime.Today);
        Form.IsActive = true;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        Form.IsEditMode = false;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await employeeService.CreateAsync(Form.ToDto(), cancellationToken);
        if (result.IsError)
        {
            ModelState.AddEmployeeErrors(result.Errors);
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
