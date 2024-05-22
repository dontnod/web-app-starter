namespace WebAppStarter.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using WebAppStarter.Application.Common.Interfaces;
using WebAppStarter.Domain.Entities;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<TodoItem> TodoItems { get; set; }
}
