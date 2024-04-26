namespace TodoApi.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using TodoApi.Context;
using TodoApi.Models;
using TodoApi.Permissions;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ToDoListController(ToDoContext toDoContext) : ControllerBase
{
    private readonly ToDoContext toDoContext = toDoContext;

    [HttpGet]
    [ReadTodoPermission]
    public async Task<ActionResult<List<ToDo>>> GetAsync()
    {
        var toDos = await toDoContext.ToDos!
            .Where(td => RequestCanAccessToDo(td.Owner))
            .ToListAsync();

        return Ok(toDos);
    }

    [HttpGet("{id}")]
    [ReadTodoPermission]
    public async Task<ActionResult<ToDo>> GetAsync(int id)
    {
        var toDo = await toDoContext.ToDos!
            .FirstOrDefaultAsync(td => RequestCanAccessToDo(td.Owner) && td.Id == id);

        if (toDo is null)
        {
            return NotFound();
        }

        return Ok(toDo);
    }

    [HttpPut("{id}")]
    [WriteTodoPermission]
    public async Task<ActionResult<ToDo>> PutAsync(int id, [FromBody] ToDo toDo)
    {
        var storedToDo = await toDoContext.ToDos!
            .FirstOrDefaultAsync(td => RequestCanAccessToDo(td.Owner) && td.Id == id);

        if (storedToDo is null)
        {
            return NotFound();
        }

        storedToDo.Description = toDo.Description;
        toDoContext.ToDos!.Update(storedToDo);

        await toDoContext.SaveChangesAsync();

        return Ok(storedToDo);
    }

    [HttpPost]
    [WriteTodoPermission]
    public async Task<ActionResult<ToDo>> PostAsync([FromBody] CreateToDoDto todoDto)
    {
        // Only let applications with global to-do access set the user ID or to-do's
        Guid? ownerIdOfTodo = IsAppMakingRequest() ? todoDto.Owner : GetUserId();

        if (!ownerIdOfTodo.HasValue)
        {
            return BadRequest("Owner is empty");
        }

        var newToDo = new ToDo()
        {
            Owner = ownerIdOfTodo.Value,
            Description = todoDto.Description,
        };

        await toDoContext.ToDos!.AddAsync(newToDo);
        await toDoContext.SaveChangesAsync();

        return Created($"/todo/{newToDo!.Id}", newToDo);
    }

    [HttpDelete("{id}")]
    [WriteTodoPermission]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var toDoToDelete = await toDoContext.ToDos!
            .FirstOrDefaultAsync(td => RequestCanAccessToDo(td.Owner) && td.Id == id);

        if (toDoToDelete is null)
        {
            return NotFound();
        }

        toDoContext.ToDos!.Remove(toDoToDelete);

        await toDoContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// The 'oid' (object id) is the only claim that should be used to uniquely identify
    /// a user in an Azure AD tenant. The token might have one or more of the following claim,
    /// that might seem like a unique identifier, but is not and should not be used as such:
    ///
    /// - upn (user principal name): might be unique amongst the active set of users in a tenant
    /// but tend to get reassigned to new employees as employees leave the organization and others
    /// take their place or might change to reflect a personal change like marriage.
    ///
    /// - email: might be unique amongst the active set of users in a tenant but tend to get reassigned
    /// to new employees as employees leave the organization and others take their place.
    /// </summary>
    private Guid GetUserId()
    {
        Guid userId;

        if (!Guid.TryParse(HttpContext.User.GetObjectId(), out userId))
        {
            throw new Exception("User ID is not valid.");
        }

        return userId;
    }

    /// <summary>
    /// Access tokens that have neither the 'scp' (for delegated permissions) nor
    /// 'roles' (for application permissions) claim are not to be honored.
    ///
    /// An access token issued by Azure AD will have at least one of the two claims. Access tokens
    /// issued to a user will have the 'scp' claim. Access tokens issued to an application will have
    /// the roles claim. Access tokens that contain both claims are issued only to users, where the scp
    /// claim designates the delegated permissions, while the roles claim designates the user's role.
    ///
    /// To determine whether an access token was issued to a user (i.e delegated) or an application
    /// more easily, we recommend enabling the optional claim 'idtyp'. For more information, see:
    /// https://docs.microsoft.com/azure/active-directory/develop/access-tokens#user-and-application-tokens
    /// </summary>
    private bool IsAppMakingRequest()
    {
        // Add in the optional 'idtyp' claim to check if the access token is coming from an application or user.
        // See: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-optional-claims
        if (HttpContext.User.Claims.Any(c => c.Type == "idtyp"))
        {
            return HttpContext.User.Claims.Any(c => c.Type == "idtyp" && c.Value == "app");
        }
        else
        {
            // alternatively, if an AT contains the roles claim but no scp claim, that indicates it's an app token
            return HttpContext.User.Claims.Any(c => c.Type == "roles") && !HttpContext.User.Claims.Any(c => c.Type == "scp");
        }
    }

    private bool RequestCanAccessToDo(Guid userId)
    {
        return IsAppMakingRequest() || (userId == GetUserId());
    }
}