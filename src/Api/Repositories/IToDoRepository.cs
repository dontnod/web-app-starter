namespace TodoApi.Repositories;

using TodoApi.Models;

public interface IToDoRepository
{
    Task<List<ToDo>> GetListByOwnerAsync(Guid owner);

    Task<List<ToDo>> GetAllAsync();

    Task<ToDo?> GetByIdAsync(int id);

    Task<ToDo> AddAsync(ToDo newToDo);

    Task<ToDo> UpdateAsync(ToDo existingToDo);

    Task<ToDo> DeleteAsync(ToDo toDoToDelete);
}