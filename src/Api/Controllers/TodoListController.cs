namespace WebAppStarter.Api.Controllers;

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppStarter.Api.Security;
using WebAppStarter.Application.TodoItems.Commands.CreateTodoItem;
using WebAppStarter.Application.TodoItems.Commands.DeleteTodoItem;
using WebAppStarter.Application.TodoItems.Commands.UpdateTodoItem;
using WebAppStarter.Application.TodoItems.Queries.GetTodoItemById;
using WebAppStarter.Application.TodoItems.Queries.GetUserTodoItems;
using WebAppStarter.Domain.Entities;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodoListController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ReadTodoPermission]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TodoItem>>> GetAllAsync()
    {
        return (await mediator.Send(new GetUserTodoItemsQuery())).ToActionResult(this);
    }

    [HttpGet("{id}")]
    [ReadTodoPermission]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<TodoItem>> GetByIdAsync(int id)
    {
        return (await mediator.Send(new GetTodoItemByIdQuery(id))).ToActionResult(this);
    }

    [HttpPut("{id}")]
    [WriteTodoPermission]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    public async Task<ActionResult<TodoItem>> UpdateAsync(int id, [FromBody][Required] UpdateTodoItemCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        return (await mediator.Send(command)).ToActionResult(this);
    }

    [HttpPost]
    [WriteTodoPermission]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TodoItem>> CreateAsync([FromBody][Required] CreateTodoItemCommand command)
    {
        return (await mediator.Send(command)).ToActionResult(this);
    }

    [HttpDelete("{id}")]
    [WriteTodoPermission]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    public async Task<ActionResult<TodoItem>> DeleteAsync(int id)
    {
        return (await mediator.Send(new DeleteTodoItemCommand(id))).ToActionResult(this);
    }
}