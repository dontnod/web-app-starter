namespace WebAppStarter.Application.FunctionalTests;

using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Infrastructure.Data;

public class InMemoryTestDatabase : ITestDatabase
{
    public Task InitialiseAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options);

        return Task.CompletedTask;
    }

    public DbConnection GetConnection()
    {
        return null!;
    }

    public async Task ResetAsync()
    {
        await InitialiseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}