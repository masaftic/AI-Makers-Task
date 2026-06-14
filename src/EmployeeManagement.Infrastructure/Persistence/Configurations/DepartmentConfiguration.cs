using EmployeeManagement.Domain.DepartmentRoot;
using EmployeeManagement.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(department => department.Id);
        builder.Property(department => department.Id).ValueGeneratedOnAdd();

        builder.Property(department => department.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(department => department.Name).IsUnique();
        builder.HasData(DepartmentSeed.All);
    }
}
