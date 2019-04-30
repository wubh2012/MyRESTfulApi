using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRESTfulApi.Models;

namespace MyRESTfulApi.Controller
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;
        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
            if (_todoContext.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _todoContext.TodoItems.Add(new TodoItem { Name = "Item1" });
                _todoContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _todoContext.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _todoContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _todoContext.TodoItems.Add(item);
            await _todoContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            _todoContext.Entry(item).State = EntityState.Modified;
            await _todoContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// 删除指定的 todoItem 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _todoContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            _todoContext.TodoItems.Remove(todoItem);
            await _todoContext.SaveChangesAsync();
            return NoContent();
        }

    }
}