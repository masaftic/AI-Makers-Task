using EmployeeManagement.Application.Employees;
using EmployeeManagement.Web.Pages.Shared;
using EmployeeManagement.Application.Departments;
using EmployeeManagement.Application.Departments.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagement.Application.Employees.Dtos;

namespace EmployeeManagement.Web.Pages;

public sealed class IndexModel(
    IEmployeeService employeeService,
    IDepartmentService departmentService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int? DepartmentId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Name { get; set; }

    public IReadOnlyList<EmployeeDto> Employees { get; private set; } = [];
    public IReadOnlyList<DepartmentDto> Departments { get; private set; } = [];

    public ConfirmationModalModel DeleteModal { get; } = new(
        ModalId: "deleteEmployeeModal",
        Title: "Delete employee",
        Message: "Are you sure you want to delete",
        Handler: "Delete",
        ConfirmButtonText: "Delete employee");

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Employees = await employeeService.GetAllAsync(Name, DepartmentId, cancellationToken);
        Departments = await departmentService.GetAllAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostDeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await employeeService.DeleteAsync(id, cancellationToken);

        return result.IsError
            ? NotFound()
            : RedirectToPage(new { name = Name, departmentId = DepartmentId });
    }
}
