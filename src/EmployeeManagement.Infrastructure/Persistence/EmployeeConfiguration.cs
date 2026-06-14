using EmployeeManagement.Domain.EmployeeRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(employee => employee.Id);
        builder.Property(employee => employee.Id).ValueGeneratedNever();

        builder.Property(employee => employee.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(employee => employee.Email)
            .HasConversion(
                email => email.Value,
                value => EmailAddress.Create(value))
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(employee => employee.MobileNumber)
            .HasConversion(
                mobileNumber => mobileNumber.Value,
                value => MobileNumber.Create(value))
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(employee => employee.JobTitle)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(employee => employee.Email).IsUnique();
        builder.HasIndex(employee => employee.MobileNumber).IsUnique();

        builder.HasData(EmployeeSeed.All);
    }
}
