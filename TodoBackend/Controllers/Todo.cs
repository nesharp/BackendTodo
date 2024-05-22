using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Models;

namespace TodoBackend.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class Todos:ControllerBase
{
    private readonly DatabaseContext _context;
    public Todos(DatabaseContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<List<Todo>> Get()
    {   string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //returning unauthorized if user id is null(this more for safe parse)
        if (userId == null) return Unauthorized();
        //getting all posts from database where user id is equal to user id from token
        List<Todo> todos = _context.Todos
            .Where(t => t.User.Id == int.Parse(userId))
            .Select(t => new Todo {
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                AssignedTo = t.AssignedTo,
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                UserId = t.UserId
            })
            .ToList();      
        return Ok(todos);
    }
    
    [HttpPost]
    public ActionResult<Todo> Create([FromBody] TodoDto todo)
    {
        //getting user id from token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //returning unauthorized if user id is null(this more for safe parse)
        if (userId == null) return Unauthorized();
        //creating new todo
        var newTodo = new Todo
        {
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted ?? false,
            AssignedTo = todo.AssignedTo,  
            UserId  = int.Parse(userId),
        };
        //adding new todo to database     
        _context.Todos.Add(newTodo);
        //saving changes
        _context.SaveChanges();
        return Ok(newTodo);
    }
    
    [HttpGet("{id}")]
    public ActionResult<Todo> Get(int id)
    {
        //getting user id from token
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //returning unauthorized if user id is null(this more for safe parse)
        if (userId == null) return Unauthorized();
        //getting todo from database where id is equal to id from url and user id is equal to user id from token
        Todo? todo = _context.Todos.FirstOrDefault(t => t.Id == id && t.User.Id == int.Parse(userId));
        //returning not found if todo is null
        if (todo == null) return NotFound();
        return Ok(todo);
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        //getting user id from token
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //returning unauthorized if user id is null(this more for safe parse)
        if (userId == null) return Unauthorized();
        //getting todo from database where id is equal to id from url and user id is equal to user id from token
        Todo? todo = _context.Todos.FirstOrDefault(t => t.Id == id && t.User.Id == int.Parse(userId));
        //returning not found if todo is null
        if (todo == null) return NotFound();
        //removing todo from database
        _context.Todos.Remove(todo);
        //saving changes
        _context.SaveChanges();
        return Ok(new
        {
            message = "Todo was deleted successfully"
        });
    }
    
    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] TodoUpdateDto todo)
    {
        //getting user id from token
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //returning unauthorized if user id is null(this more for safe parse)
        if (userId == null) return Unauthorized();
        //getting todo from database where id is equal to id from url and user id is equal to user id from token
        Todo? todoFromDb = _context.Todos.FirstOrDefault(t => t.Id == id && t.User.Id == int.Parse(userId));
        //returning not found if todo is null
        if (todoFromDb == null) return NotFound();
        // //updating todo
        todoFromDb.Title = todo.Title ?? todoFromDb.Title;
        todoFromDb.Description = todo.Description ?? todoFromDb.Description;
        todoFromDb.AssignedTo = todo.AssignedTo;
        
        //saving changes
        _context.SaveChanges();
        return Ok(todoFromDb);
    }
    
    [HttpPost("{id}/state")]
    public ActionResult ChangeState(int id, [FromBody] ChangeStateDto state)
    {
        //getting user id from token
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //returning unauthorized if user id is null(this more for safe parse)
        if (userId == null) return Unauthorized();
        //getting todo from database where id is equal to id from url and user id is equal to user id from token
        Todo? todo = _context.Todos.FirstOrDefault(t => t.Id == id && t.User.Id == int.Parse(userId));
        //returning not found if todo is null
        if (todo == null) return NotFound();
        //changing state of todo
        todo.IsCompleted = state.IsCompleted;
        //saving changes
        _context.SaveChanges();
        return Ok(todo);
    }
}