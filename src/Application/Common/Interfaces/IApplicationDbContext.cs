namespace WebAppStarter.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using WebAppStarter.Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
