using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Models;
using ToDo.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly ILogger<TodosController> _logger;
        public TodosController(IToDoService toDoService, ILogger<TodosController> logger)
        {
            _toDoService = toDoService;
            _logger = logger;
        }

        // GET: api/Todos
        [HttpGet(Name = "GetAllTodos")]
        public async Task<ActionResult<IEnumerable<ToDoModel>>> Get()
        {
            _logger.LogInformation("GET: api/Todos");
            try
            {
                var output = await _toDoService.GetAllTodos();

                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to api/Todos failed.");
                return BadRequest(ex.Message);
            }
        }

        // GET api/Todos/5
        [HttpGet("{id}", Name = "GetSpecificTodo")]
        public async Task<ActionResult<ToDoModel>> Get(int id)
        {
            _logger.LogInformation("GET: api/Todos/{id}", id);
            try
            {
                var output = await _toDoService.GetTodo(id);

                if (output == null)
                {
                    return NotFound();
                }

                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to {ApiPath} failed. (Id: {id})", "api/Todos/id", id);
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Todos/GetTodosForCurrentDay
        [HttpGet("GetForCurrentDay", Name = "GetTodosForCurrentDay")]
        public async Task<ActionResult<IEnumerable<ToDoModel>>> GetTodosForCurrentDay()
        {
            _logger.LogInformation("GET: api/GetForCurrentDay");
            try
            {
                var output = await _toDoService.GetIncomingToDosForCurrentDay();

                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to api/GetForCurrentDay failed.");
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Todos/GetTodosForNextDay
        [HttpGet("GetForNextDay", Name = "GetTodosForNextDay")]
        public async Task<ActionResult<IEnumerable<ToDoModel>>> GetTodosForNextDay()
        {
            _logger.LogInformation("GET: api/Todos/GetForNextDay");
            try
            {
                var output = await _toDoService.GetIncomingToDosForNextDay();

                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to api/Todos/GetForNextDay failed.");
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Todos/GetToDosForCurrentWeek
        [HttpGet("GetForCurrentWeek", Name = "GetToDosForCurrentWeek")]
        public async Task<ActionResult<IEnumerable<ToDoModel>>> GetToDosForCurrentWeek()
        {
            _logger.LogInformation("GET: api/Todos/GetForCurrentWeek");
            try
            {
                var output = await _toDoService.GetToDosForCurrentWeek();

                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The GET call to api/Todos/GetForCurrentWeek failed.");
                return BadRequest(ex.Message);
            }
        }

        // POST api/Todos
        [HttpPost(Name = "CreateTodo")]
        public async Task<ActionResult> Post([FromBody] ToDoModel toDo)
        {
            _logger.LogInformation("POST: api/Todos (Task: {toDo})", toDo);
            try
            {
                await _toDoService.CreateToDo(toDo);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The POST call to api/Todos failed. (toDo: {toDo})", toDo);
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Todos
        [HttpPut(Name = "UpdateTodo")]
        public async Task<IActionResult> Put([FromBody] ToDoModel toDo)
        {
            _logger.LogInformation("PUT: api/Todos (toDo: {toDo})", toDo);
            try
            {
                await _toDoService.UpdateToDo(toDo);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/Todos failed. (toDo: {toDo})", toDo);
                return BadRequest(ex.Message);
            }
        }

        // PATCH api/Todos/5/Complete
        [HttpPatch("{id}/Complete", Name = "CompleteTodo")]
        public async Task<IActionResult> Complete(int id)
        {
            _logger.LogInformation("PUT: api/Todos/{id}/Complete", id);
            try
            {
                await _toDoService.CompleteToDo(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/Todos/{id}/Complete failed.)", id);
                return BadRequest(ex.Message);
            }
        }

        // PATCH api/Todos/5/SetPercentComplete/50
        [HttpPatch("{id}/SetPercentComplete/{percent}", Name = "SetPercentComplete")]
        public async Task<IActionResult> SetPercentComplete(int id, int percent)
        {
            _logger.LogInformation("PUT: api/Todos/{id}/SetPercentComplete/{percent}", id, percent);
            try
            {
                await _toDoService.SetToDoPercentComplete(id, percent);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/Todos/{id}/SetPercentComplete/{percent} failed.)", id, percent);
                return BadRequest(ex.Message);
            }
        }


        // DELETE api/Todos/5
        [HttpDelete("{id}", Name = "DeleteTodo")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("PUT: api.Todos/{id}/Delete", id);
            try
            {
                await _toDoService.DeleteToDo(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The DELETE call to api/Todos/{id} failed.", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
