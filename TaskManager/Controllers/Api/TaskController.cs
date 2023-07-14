using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var tasks = await _taskRepository.GetTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}", Name = "TaskById")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskById(id);
                if(task == null)
                {
                    return NotFound();
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(Models.Task task)
        {
            try
            {
                var createdTask = await _taskRepository.CreateTask(task);
                return CreatedAtRoute(task.Title, new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            try
            {
                var dbTask = await _taskRepository.GetTaskById(id);
                if(dbTask == null)
                {
                    return NotFound();
                }

                await _taskRepository.UpdateTask(id, task);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var dbTask = await _taskRepository.GetTaskById(id);
                if(dbTask == null)
                {
                    return NotFound();
                }
                await _taskRepository.DeleteTask(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
