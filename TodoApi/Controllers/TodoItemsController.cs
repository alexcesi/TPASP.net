using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using MvcMusic.Data;
using MvcMusic.Models;

namespace TodoApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly MvcMusicContext _context;

        public TodoItemsController(MvcMusicContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.Music
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.Music.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.Music.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Id = todoItemDTO.Id;
            todoItem.Title = todoItemDTO.Title;
            todoItem.ReleaseDate = todoItemDTO.ReleaseDate;
            todoItem.Genre = todoItemDTO.Genre;
            todoItem.Price = todoItemDTO.Price;
            todoItem.Rating = todoItemDTO.Rating;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new Music
            {
            Id = todoItemDTO.Id,
            Title = todoItemDTO.Title,
            ReleaseDate = todoItemDTO.ReleaseDate,
            Genre = todoItemDTO.Genre,
            Price = todoItemDTO.Price,
            Rating = todoItemDTO.Rating,
        };

            _context.Music.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.Music.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Music.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id) =>
             _context.Music.Any(e => e.Id == id);

        private static TodoItemDTO ItemToDTO(Music music) =>
            new TodoItemDTO
            {
                Id = music.Id,
                Title = music.Title,
                ReleaseDate = music.ReleaseDate,
                Genre = music.Genre,
                Price = music.Price,
                Rating = music.Rating,

            };
    }
 }
