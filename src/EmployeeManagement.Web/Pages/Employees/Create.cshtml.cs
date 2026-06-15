using EmployeeManagement.Application.Departments;
using EmployeeManagement.Application.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Web.Pages.Employees;

public sealed class CreateModel(
    IEmployeeService employeeService,
    IDepartmentService departmentService) : PageModel
{
    [BindProperty]
    public EmployeeFormModel Form { get; set; } = new();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Form.HireDate = DateOnly.FromDateTime(DateTime.Today);
        Form.IsActive = true;
        await LoadDepartmentsAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        Form.IsEditMode = false;

        if (!ModelState.IsValid)
        {
            await LoadDepartmentsAsync(cancellationToken);
            return Page();
        }

        var result = await employeeService.CreateAsync(Form.ToDto(), cancellationToken);
        if (result.IsError)
        {
            ModelState.AddEmployeeErrors(result.Errors);
            await LoadDepartmentsAsync(cancellationToken);
            return Page();
        }

        return RedirectToPage("/Index");
    }

    private async Task LoadDepartmentsAsync(CancellationToken cancellationToken)
    {
        Form.Departments = await departmentService.GetAllAsync(cancellationToken);
    }
}
