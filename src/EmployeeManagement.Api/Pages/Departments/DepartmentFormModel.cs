using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Application.Departments;
using EmployeeManagement.Application.Departments.Dtos;

namespace EmployeeManagement.Api.Pages.Departments;

public sealed class DepartmentFormModel
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; } = "";

    public bool IsEditMode { get; set; }

    public string Heading => IsEditMode ? "Edit department" : "Create department";
    public string SubmitButtonText => IsEditMode ? "Save changes" : "Create department";

    public SaveDepartmentDto ToDto() => new(Name);

    public static DepartmentFormModel FromDto(DepartmentDto department) =>
        new()
        {
            Name = department.Name,
            IsEditMode = true
        };
}
