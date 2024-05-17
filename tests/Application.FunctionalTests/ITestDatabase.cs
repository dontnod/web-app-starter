namespace WebAppStarter.Application.FunctionalTests;

using System.Data.Common;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task ResetAsync();

    Task DisposeAsync();
}
