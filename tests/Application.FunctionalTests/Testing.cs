namespace WebAppStarter.Application.FunctionalTests;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAppStarter.Infrastructure.Data;

[SetUpFixture]
public partial class Testing
{
    private static ITestDatabase database;
    private static CustomWebApplicationFactory factory = null!;
    private static IServiceScopeFactory scopeFactory = null!;
    private static Guid? userId;
    private static bool isApplication = false;

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task SendAsync(IBaseRequest request)
    {
        using var scope = scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public static Guid? GetUserId()
    {
        return userId;
    }

    public static string? GetDisplayName()
    {
        return "TestUser";
    }

    public static bool IsApplication()
    {
        return isApplication;
    }

    public static Task<Guid?> RunAsDefaultUserAsync()
    {
        userId = Guid.NewGuid();

        return Task.FromResult(userId);
    }

    public static Task<Guid?> RunAsApplicationAsync()
    {
        userId = Guid.NewGuid();
        isApplication = true;

        return Task.FromResult(userId);
    }

    public static async Task ResetState()
    {
        try
        {
            await database.ResetAsync();
        }
        catch (Exception) { }

        userId = null;
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>()
        where TEntity : class
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        database = await TestDatabaseFactory.CreateAsync();

        factory = new CustomWebApplicationFactory(database.GetConnection());

        scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await database.DisposeAsync();
        await factory.DisposeAsync();
    }
}
