namespace WebAppStarter.Application.FunctionalTests;

using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Infrastructure.Data;
using static Testing;

#pragma warning disable CS9113 // Parameter is unread.
public class CustomWebApplicationFactory(DbConnection connection) : WebApplicationFactory<Program>
#pragma warning restore CS9113 // Parameter is unread.
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<ICurrentUser>()
                .AddTransient(provider =>
                    Mock.Of<ICurrentUser>(s =>
                        s.GetId() == GetUserId()
                        && s.GetDisplayName() == GetDisplayName()
                        && s.IsApplication() == IsApplication()
                    )
                );

            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>(
                    (sp, options) =>
                    {
                        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                        options.UseSqlite(connection);
                    }
                );
        });
    }
}
