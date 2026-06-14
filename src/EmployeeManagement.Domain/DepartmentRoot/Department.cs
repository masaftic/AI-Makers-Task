using Ardalis.GuardClauses;

namespace EmployeeManagement.Domain.DepartmentRoot;

public sealed class Department
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = null!;

    private Department() { }

    private Department(Guid id, string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        Id = id;
        Name = name;
    }

    public static Department Create(Guid id, string name) => new(id, name);

    public void Update(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Name = name;
    }
}
