namespace DNE.Todo.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Repositories;

public class ToDoListControllerTestsWithUser
{
    private readonly ToDoListController controller;
    private readonly Mock<IToDoRepository> mockService;
    private readonly Guid userId = Guid.NewGuid(); // Example user ID

    public ToDoListControllerTestsWithUser()
    {
        mockService = new Mock<IToDoRepository>();
        controller = new ToDoListController(mockService.Object)
        {
            // Mocking HttpContext to simulate user identity and claims
            ControllerContext = new ControllerContext()
            {
                HttpContext = MockHttpContext.CreateUserContext(userId),
            },
        };
    }

    [Fact]
    public async Task GetAsync_ReturnsUserToDos_IfUserRequest()
    {
        var userTodo = new ToDo { Owner = userId, Description = "Task 1" };
        var toDos = new List<ToDo>
        {
           userTodo,
           new ToDo { Owner = Guid.NewGuid(), Description = "Task of an other user" },
           new ToDo { Owner = Guid.NewGuid(), Description = "Task of an other user" },
        };
        mockService.Setup(s => s.GetListByOwnerAsync(userId)).ReturnsAsync(toDos.Where(td => td.Owner == userId).ToList());

        var result = await controller.GetAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedToDos = Assert.IsType<List<ToDo>>(okResult.Value);
        Assert.Single(returnedToDos);
        Assert.True(returnedToDos.FirstOrDefault() == userTodo);
    }

    [Fact]
    public async Task GetAsyncById_ReturnsNotFound_IfToDoDoesNotExist()
    {
        mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(value: null);

        var result = await controller.GetAsync(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostAsync_CreatesToDo_ReturnsCreatedResponse()
    {
        var todoDto = new CreateToDoDto { Owner = userId, Description = "New Task" };
        var newToDo = new ToDo { Id = 0, Owner = userId, Description = "New Task" };

        mockService.Setup(s => s.AddAsync(It.IsAny<ToDo>())).Returns(Task.FromResult(newToDo));

        var result = await controller.PostAsync(todoDto);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(newToDo, createdResult.Value);
    }
}
