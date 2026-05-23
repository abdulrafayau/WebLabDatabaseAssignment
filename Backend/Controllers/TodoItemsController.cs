using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAPI.Data;
using WebLabAPI.Models;

namespace WebLabAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetUserTodos(int userId)
    {
        var todos = await _context.TodoItems
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return todos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null)
            return NotFound(new { message = "Todo item not found" });
        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodoItem(CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required" });

        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
            return BadRequest(new { message = "User not found" });

        var todo = new TodoItem
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItem), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoItem(int id, UpdateTodoRequest request)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null)
            return NotFound(new { message = "Todo item not found" });

        todo.Title = request.Title ?? todo.Title;
        todo.Description = request.Description ?? todo.Description;
        todo.DueDate = request.DueDate ?? todo.DueDate;
        todo.IsCompleted = request.IsCompleted ?? todo.IsCompleted;
        todo.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Update failed" });
        }

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null)
            return NotFound(new { message = "Todo item not found" });

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Todo item deleted successfully" });
    }
}

public class CreateTodoRequest
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateTodoRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool? IsCompleted { get; set; }
}
