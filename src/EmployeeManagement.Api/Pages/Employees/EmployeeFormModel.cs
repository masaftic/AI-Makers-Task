using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Api.Validation;
using EmployeeManagement.Application.Employees;

namespace EmployeeManagement.Api.Pages.Employees;

public sealed class EmployeeFormModel
{
    [Required]
    [StringLength(200)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = "";

    [Required]
    [DomainEmailAddress]
    [StringLength(320)]
    public string Email { get; set; } = "";

    [Required]
    [DomainMobileNumber]
    [Display(Name = "Mobile number")]
    public string MobileNumber { get; set; } = "";

    [Required]
    [Display(Name = "Department ID")]
    public Guid DepartmentId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Job title")]
    public string JobTitle { get; set; } = "";

    [Required]
    [Display(Name = "Hire date")]
    [DataType(DataType.Date)]
    public DateOnly HireDate { get; set; }

    [Display(Name = "Active employee")]
    public bool IsActive { get; set; }

    public bool IsEditMode { get; set; }

    public string Heading => IsEditMode ? "Edit employee" : "Create employee";
    public string SubmitButtonText => IsEditMode ? "Save changes" : "Create employee";

    public SaveEmployeeDto ToDto() =>
        new(FullName, Email, MobileNumber, DepartmentId, JobTitle, HireDate, IsActive);

    public static EmployeeFormModel FromDto(EmployeeDto employee) =>
        new()
        {
            FullName = employee.FullName,
            Email = employee.Email,
            MobileNumber = employee.MobileNumber,
            DepartmentId = employee.DepartmentId,
            JobTitle = employee.JobTitle,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive,
            IsEditMode = true
        };
}
