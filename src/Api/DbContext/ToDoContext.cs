namespace TodoApi.Context;

using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class ToDoContext : DbContext
{
    public ToDoContext(DbContextOptions<ToDoContext> options)
        : base(options)
    {
    }

    public DbSet<ToDo> ToDos { get; set; }
}
