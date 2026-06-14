using EmployeeManagement.Application.Common;
using EmployeeManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<EmployeeManagementDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EmployeeManagement")));
            services.AddScoped<IAppDbContext>(serviceProvider =>
                serviceProvider.GetRequiredService<EmployeeManagementDbContext>());

            return services;
        }
    }

    extension(IServiceProvider services)
    {
        public async Task InitializeDatabaseAsync()
        {
            await using var scope = services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<EmployeeManagementDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}
