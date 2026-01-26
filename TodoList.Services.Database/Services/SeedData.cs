using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TodoList.Services.Database.Context;
using TodoList.Services.Database.Entities;
using TodoList.Services.Database.Helpers;
using TodoList.Services.Database.Identity;
using TodoList.WebApi.Models.Enums;

public static class SeedData
{
    public static async Task InitializeAsync(
        TodoListDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger logger)
    {
        try
        {
            var defaultEmail = "demo@demo.com";
            var defaultPassword = "Password123!";

            var user = await userManager.FindByEmailAsync(defaultEmail);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = defaultEmail,
                    Email = defaultEmail,
                };

                var result = await userManager.CreateAsync(user, defaultPassword);
                if (!result.Succeeded)
                {
                    logger.LogError(
                        "Failed to create default user. Errors: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));

                    throw new Exception("Could not create default user for seeding.");
                }

                logger.LogInformation("Default user created: {Email}", defaultEmail);
            }

            if (context.TodoLists.Any())
            {
                logger.LogInformation("TodoLists already exist — skipping seeding.");
                return;
            }

            logger.LogInformation("Seeding todo lists");

            // 1. Create TodoLists (NO UserId)
            var todoLists = new List<TodoListEntity>
            {
                new () { Title = "Personal", Description = "Personal goals" },
                new () { Title = "Work", Description = "Work projects" },
                new () { Title = "Shopping", Description = "Groceries and errands" },
                new () { Title = "Fitness", Description = "Workout and health plans" },
                new () { Title = "Travel", Description = "Trips and planning" },
            };

            await context.TodoLists.AddRangeAsync(todoLists);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} todo lists.", todoLists.Count);

            // 2. Assign OWNER role via join table
            var memberships = todoLists.Select(list => TodoListUserMapper.ToEntity(list.Id, user.Id, TodoListRole.Owner));

            await context.TodoListUsers.AddRangeAsync(memberships);
            await context.SaveChangesAsync();

            logger.LogInformation("Assigned OWNER role to user {UserId}", user.Id);

            // 3. Seed Tasks
            var tasks = new List<TaskEntity>
            {
                new ()
                {
                    Title = "Morning run",
                    Description = "5km jog",
                    TodoListId = todoLists.First(l => l.Title == "Fitness").Id
                },
                new ()
                {
                    Title = "Buy groceries",
                    Description = "Milk, eggs, bread",
                    TodoListId = todoLists.First(l => l.Title == "Shopping").Id
                },
                new ()
                {
                    Title = "Finish report",
                    Description = "End-of-day work report",
                    TodoListId = todoLists.First(l => l.Title == "Work").Id
                },
                new ()
                {
                    Title = "Book hotel",
                    Description = "Summer vacation hotel",
                    TodoListId = todoLists.First(l => l.Title == "Travel").Id
                },
                new ()
                {
                    Title = "Call mom",
                    Description = "Weekly catch-up",
                    TodoListId = todoLists.First(l => l.Title == "Personal").Id
                },
            };

            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} tasks.", tasks.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SeedData.InitializeAsync failed.");
            throw;
        }
    }
}
