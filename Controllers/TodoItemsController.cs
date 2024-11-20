using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class TodoItemsController : ControllerBase {
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context) {
      _context = context;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems() {

      //Get TODOs with Comments (include = join) from DB
      var todoItems = await _context.TodoItems
          .Include(todoItem => todoItem.Comments)
          .ToListAsync();

      //Map the TodoItems in DTOs
      var todoItemsDto = todoItems
          .Select(todoItem => new TodoItemDto(todoItem))
          .ToList();

      //Wraps DTO in ObjectResult
      return Ok(todoItemsDto);

    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id) {
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null) {
        return NotFound();
      }

      return todoItem;
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem) {
      if (id != todoItem.Id) {
        return BadRequest();
      }

      _context.Entry(todoItem).State = EntityState.Modified;

      try {
        await _context.SaveChangesAsync();
      } catch (DbUpdateConcurrencyException) {
        if (!TodoItemExists(id)) {
          return NotFound();
        } else {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem) {
      _context.TodoItems.Add(todoItem);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id) {
      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null) {
        return NotFound();
      }

      _context.TodoItems.Remove(todoItem);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TodoItemExists(long id) {
      return _context.TodoItems.Any(e => e.Id == id);
    }

    [HttpGet("{todoItemId}/comments")]
    [Tags("Comments")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetComments(int todoItemId) {
      var todoItem = await _context.TodoItems
                                   .Include(t => t.Comments)
                                   .FirstOrDefaultAsync(t => t.Id == todoItemId);
      if (todoItem == null)
        return NotFound();

      return Ok(todoItem.Comments);
    }

    [HttpPost("{todoItemId}/comments")]
    [Tags("Comments")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> PostComments(int todoItemId, [FromBody] Comment comment) {
      var todoItem = await _context.TodoItems
                                   .Include(t => t.Comments)
                                   .FirstOrDefaultAsync(t => t.Id == todoItemId);
      if (todoItem == null)
        return NotFound();

      todoItem.Comments.Add(comment);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetComments", new { id = todoItem.Id }, comment);
    }


  }
}
