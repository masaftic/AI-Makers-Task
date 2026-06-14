using EmployeeManagement.Application.Employees;
using EmployeeManagement.Api.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagement.Application.Employees.Dtos;

namespace EmployeeManagement.Api.Pages;

public sealed class IndexModel(IEmployeeService employeeService) : PageModel
{
    public IReadOnlyList<EmployeeDto> Employees { get; private set; } = [];

    public ConfirmationModalModel DeleteModal { get; } = new(
        ModalId: "deleteEmployeeModal",
        Title: "Delete employee",
        Message: "Are you sure you want to delete",
        Handler: "Delete",
        ConfirmButtonText: "Delete employee");

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Employees = await employeeService.GetAllAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostDeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await employeeService.DeleteAsync(id, cancellationToken);

        return result.IsError
            ? NotFound()
            : RedirectToPage();
    }
}
