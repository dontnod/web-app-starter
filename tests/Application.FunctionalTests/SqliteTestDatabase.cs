namespace WebAppStarter.Application.FunctionalTests;

using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebAppStarter.Infrastructure.Data;

public class SqliteTestDatabase : ITestDatabase
{
    private readonly string connectionString;
    private readonly SqliteConnection connection;

    public SqliteTestDatabase()
    {
        connectionString = "DataSource=:memory:";
        connection = new SqliteConnection(connectionString);
    }

    public async Task InitialiseAsync()
    {
        if (connection.State == ConnectionState.Open)
        {
            await connection.CloseAsync();
        }

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.Migrate();
    }

    public DbConnection GetConnection()
    {
        return connection;
    }

    public async Task ResetAsync()
    {
        await InitialiseAsync();
    }

    public async Task DisposeAsync()
    {
        await connection.DisposeAsync();
    }
}