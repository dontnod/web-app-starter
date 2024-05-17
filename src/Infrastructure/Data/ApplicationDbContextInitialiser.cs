namespace WebAppStarter.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAppStarter.Domain.Entities;

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
{
#if USE_SQL_LITE
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
#endif

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!context.TodoItems.Any())
        {
            context.TodoItems.AddRange([
                new TodoItem { Description = "Make a todo list 📃" },
                new TodoItem { Description = "Check off the first item ✅" },
                new TodoItem { Description = "Realise you've already done two things on the list! 🤯" },
                new TodoItem { Description = "Reward yourself with a nice, long nap 🏆" },
            ]);

            await context.SaveChangesAsync();
        }
    }
}
