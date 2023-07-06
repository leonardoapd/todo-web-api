using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Entities;
using ToDoBackend.Repositories;

namespace ToDoBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class TodoController : ControllerBase
    {
        private readonly IToDoRepository _todoRepository;

        public TodoController(IToDoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllToDos()
        {
            try
            {
                var todos = await _todoRepository.GetAllToDosAsync();
                return Ok(todos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{userEmail}")]
        public async Task<IActionResult> GetAllToDosByUserEmail(string userEmail)
        {
            try
            {
                var todos = await _todoRepository.GetAllToDosByUserEmailAsync(userEmail);
                if (todos == null)
                {
                    return NotFound();
                }
                return Ok(todos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDo(ToDo toDo)
        {
            try
            {
                await _todoRepository.CreateToDoAsync(toDo);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoById(Guid id, ToDo toDo)
        {
            try
            {
                var existingToDo = await _todoRepository.GetToDoByIdAsync(id);
                if (existingToDo == null)
                {
                    return NotFound();
                }
                await _todoRepository.UpdateToDoAsync(id, toDo);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoById(Guid id)
        {
            try
            {
                var existingToDo = await _todoRepository.GetToDoByIdAsync(id);
                if (existingToDo == null)
                {
                    return NotFound();
                }
                await _todoRepository.DeleteToDoAsync(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CompleteToDoById(Guid id, ToDo toDo)
        {
            try
            {
                var existingToDo = await _todoRepository.GetToDoByIdAsync(id);
                if (existingToDo == null)
                {
                    return NotFound();
                }
                await _todoRepository.CompleteToDoAsync(id, toDo);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}