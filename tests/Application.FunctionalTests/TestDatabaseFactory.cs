namespace WebAppStarter.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
#if (USE_SQL_LITE)
        var database = new SqliteTestDatabase();
#else

        var database = new InMemoryTestDatabase();

#endif

        await database.InitialiseAsync();

        return database;
    }
}
