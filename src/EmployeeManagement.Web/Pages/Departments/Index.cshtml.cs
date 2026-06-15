using EmployeeManagement.Web.Pages.Shared;
using EmployeeManagement.Application.Departments;
using EmployeeManagement.Application.Departments.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Web.Pages.Departments;

public sealed class IndexModel(IDepartmentService departmentService) : PageModel
{
    public IReadOnlyList<DepartmentDto> Departments { get; private set; } = [];

    public ConfirmationModalModel DeleteModal { get; } = new(
        ModalId: "deleteDepartmentModal",
        Title: "Delete department",
        Message: "Are you sure you want to delete",
        Handler: "Delete",
        ConfirmButtonText: "Delete department");

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Departments = await departmentService.GetAllAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostDeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await departmentService.DeleteAsync(id, cancellationToken);
        return result.IsError ? NotFound() : RedirectToPage();
    }
}
