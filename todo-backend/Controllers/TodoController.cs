using System.Security.Claims;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using todo_backend.Dto;
using todo_backend.Models;
using todo_backend.Services.Interface;
using todo_backend.Validator;

namespace todo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    [Authorize]
    public class TodoController(ITodoService TodoService, ILogger<TodoController> logger, IHttpContextAccessor context) : Controller
    {

        private readonly ITodoService _todoService = TodoService;
        private readonly ILogger<TodoController> _logger = logger;
        private readonly IHttpContextAccessor _contextAccessor = context;
        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Todo> > GetTodoList()
        {
            
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var todoitems = await _todoService.GetTodos(userId);
                    return Ok(todoitems);
                }
                else  { return StatusCode(500); } 
            }
            catch (Exception ex) 
            { return StatusCode(500, ex); }
        }
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType( StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Todo> > CreateTodo(CreateTodoRequest request )
        {
            TodoValidator validator = new();
            ValidationResult result = validator.Validate(request);
            if (result.IsValid)
            {
                try
                {

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var todoitem = await _todoService.CreateTodo(request, userId);
                        return CreatedAtAction(nameof(CreateTodo), todoitem);
                    }
                    else { return StatusCode(500, "User not Logged in"); }
                }
                catch (Exception ex)
                { return StatusCode(500, ex); }
            }
            else
            {
                return BadRequest(result.Errors.FirstOrDefault().ErrorMessage);
            }
            
        }
        [HttpDelete("{id}")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType( StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Todo> > DeleteTodo(string id )
        {
            if (MongoValidation.IsValidMongoId(id))
            {
                try { 
                    var todoitem = await _todoService.DeleteTodo(id);
                    if(todoitem == null)
                    {
                        return NotFound();
                    }
                    return todoitem;
            
                }
                catch (Exception ex) 
                { 
                    return StatusCode(500, ex); 
                }
            }
            else
            {
                return NotFound("Invalid Id");
            }
        }
        [HttpPut("{id}")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Todo>> UpdateTodo(string id, UpdateTodoRequest dto)
        {
            if (MongoValidation.IsValidMongoId(id))
            {

                try
                {
                    var todoitem = await _todoService.UpdateTodo(id, dto);
                    return todoitem;

                }
                catch (Exception ex)
                { return StatusCode(500, ex); }
            } 
            else { 
                return BadRequest("Invalid Id"); 
            }
        }
    }
}
