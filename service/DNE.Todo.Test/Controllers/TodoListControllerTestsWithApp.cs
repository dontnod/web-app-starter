namespace DNE.Todo.Test.Controllers;

using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Repositories;

public class ToDoListControllerTestsWithApp
{
    private readonly ToDoListController controller;
    private readonly Mock<IToDoRepository> mockService;
    private readonly Guid appId = Guid.NewGuid(); // Example user ID

    public ToDoListControllerTestsWithApp()
    {
        mockService = new Mock<IToDoRepository>();
        controller = new ToDoListController(mockService.Object)
        {
            // Mocking HttpContext to simulate user identity and claims
            ControllerContext = new ControllerContext()
            {
                HttpContext = MockHttpContext.CreateAppContext(appId),
            },
        };
    }

    [Fact]
    public async Task GetAsync_ReturnsAllToDos_IfAppRequest()
    {
        var toDos = new List<ToDo>
        {
            new ToDo { Owner = Guid.NewGuid(), Description = "Task 1" },
            new ToDo { Owner = Guid.NewGuid(), Description = "Task of an other user" },
        };
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(toDos);

        var result = await controller.GetAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedToDos = Assert.IsType<List<ToDo>>(okResult.Value);
        Assert.Equal(toDos, returnedToDos);
    }

}
