using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.Departments;
using EmployeeManagement.Application.Employees;
using EmployeeManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddDbContext<EmployeeManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagement")));
builder.Services.AddScoped<IAppDbContext>(services =>
    services.GetRequiredService<EmployeeManagementDbContext>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
app.MapRazorPages();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeManagementDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();
