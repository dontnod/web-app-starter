namespace TodoApi.Repositories;

using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;

public class ToDoRepository(ToDoContext toDoContext) : IToDoRepository
{
  private readonly ToDoContext toDoContext = toDoContext;

  public async Task<List<ToDo>> GetListByOwnerAsync(Guid owner)
  {
    var toDos = await toDoContext.ToDos
      .Where(td => td.Owner == owner)
      .ToListAsync();

    return toDos;
  }

  public async Task<List<ToDo>> GetAllAsync()
  {
    var toDos = await toDoContext.ToDos
      .ToListAsync();

    return toDos;
  }

  public async Task<ToDo?> GetByIdAsync(int id)
  {
    return await toDoContext.ToDos!
        .FirstOrDefaultAsync(td => td.Id == id);
  }

  public async Task<ToDo> AddAsync(ToDo newToDo)
  {
    var addedTodo = await toDoContext.ToDos!.AddAsync(newToDo);
    await toDoContext.SaveChangesAsync();

    return addedTodo.Entity;
  }

  public async Task<ToDo> UpdateAsync(ToDo existingToDo)
  {
    var updatedTodo = toDoContext.ToDos!.Update(existingToDo);
    await toDoContext.SaveChangesAsync();

    return updatedTodo.Entity;
  }

  public async Task<ToDo> DeleteAsync(ToDo toDoToDelete)
  {
    var deletedTodo = toDoContext.ToDos!.Remove(toDoToDelete);
    await toDoContext.SaveChangesAsync();

    return deletedTodo.Entity;
  }
}