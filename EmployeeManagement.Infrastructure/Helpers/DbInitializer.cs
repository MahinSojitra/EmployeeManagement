using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string[] roleNames = { "Admin", "Employee" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }

        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            await SeedRolesAsync(serviceProvider);
            await SeedDepartmentsAsync(dbContext);
            await SeedPositionsAsync(dbContext);

            const string adminEmail = "admin@codetrade.io";
            const string adminUserName = "admin";
            const string adminPassword = "Admin@123";

            var adminRoleName = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(adminRoleName));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Find the "System Administration" department, or create if missing
                var systemAdminDept = await dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Name == "System Administration");
                if (systemAdminDept == null)
                {
                    systemAdminDept = new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = "System Administration"
                    };
                    dbContext.Departments.Add(systemAdminDept);
                    await dbContext.SaveChangesAsync();
                }

                // Find the "System Administrator" position, or create if missing
                var systemAdminPosition = await dbContext.Positions
                    .FirstOrDefaultAsync(p => p.Title == "System Administrator");
                if (systemAdminPosition == null)
                {
                    systemAdminPosition = new Position
                    {
                        Id = Guid.NewGuid(),
                        Title = "System Administrator",
                        Description = "Manages and maintains IT systems."
                    };
                    dbContext.Positions.Add(systemAdminPosition);
                    await dbContext.SaveChangesAsync();
                }

                var admin = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    UserName = adminUserName,
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfBirth = new DateTime(2003, 03, 16),
                    HireDate = DateTime.UtcNow,
                    EmployeeType = EmployeeType.FullTime,
                    DepartmentId = systemAdminDept.Id,
                    PositionId = systemAdminPosition.Id,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRoleName);
                }
                else
                {
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        public static async Task SeedDepartmentsAsync(AppDbContext context)
        {
            if (!await context.Departments.AnyAsync())
            {
                var departments = new List<Department>
                {
                    new Department { Id = Guid.NewGuid(), Name = "Software Development" },
                    new Department { Id = Guid.NewGuid(), Name = "Quality Assurance" },
                    new Department { Id = Guid.NewGuid(), Name = "DevOps" },
                    new Department { Id = Guid.NewGuid(), Name = "Human Resources" },
                    new Department { Id = Guid.NewGuid(), Name = "IT Support" },
                    new Department { Id = Guid.NewGuid(), Name = "Product Management" },
                    new Department { Id = Guid.NewGuid(), Name = "System Administration" },
                };

                context.Departments.AddRange(departments);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedPositionsAsync(AppDbContext context)
        {
            if (!await context.Positions.AnyAsync())
            {
                var positions = new List<Position>
                {
                    new Position { Id = Guid.NewGuid(), Title = "Software Engineer", Description = "Develops and maintains software applications." },
                    new Position { Id = Guid.NewGuid(), Title = "QA Analyst", Description = "Ensures quality of software through testing." },
                    new Position { Id = Guid.NewGuid(), Title = "DevOps Engineer", Description = "Manages infrastructure and deployment pipelines." },
                    new Position { Id = Guid.NewGuid(), Title = "HR Specialist", Description = "Handles recruitment and employee relations." },
                    new Position { Id = Guid.NewGuid(), Title = "IT Support Technician", Description = "Provides technical support and troubleshooting." },
                    new Position { Id = Guid.NewGuid(), Title = "Product Manager", Description = "Leads product planning and execution." },
                    new Position { Id = Guid.NewGuid(), Title = "System Administrator", Description = "Manages and maintains IT systems." },
                };

                context.Positions.AddRange(positions);
                await context.SaveChangesAsync();
            }
        }
    }
}