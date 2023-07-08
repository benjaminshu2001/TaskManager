using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Diagnostics;
using TaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        //Plan on implementing CRUD instructions 

        //First implementing dependency injection
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        //Action method to display task creation form
        public IActionResult Index()
        {
            return View();
        }

        // Action method to handle the Create Task form submission
        [HttpPost]
        public IActionResult Create(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _taskRepository.AddTask(task);
                return RedirectToAction("KanbanBoard");
            }
            return View(task);
        }

        // Action method to display the Kanban Board page
        //public IActionResult KanbanBoard()
        //{
        //    var tasks = _taskRepository.AllTasks;
        //    return View(tasks);
        //}
        public IActionResult KanbanBoard()
        {
            var allTasks = _taskRepository.AllTasks;

            var assignedTasks = allTasks.Where(t => t.Status == "Assigned");
            var inProgressTasks = allTasks.Where(t => t.Status == "InProgress");
            var completedTasks = allTasks.Where(t => t.Status == "Completed");

            ViewData["AssignedTasks"] = assignedTasks;
            ViewData["InProgressTasks"] = inProgressTasks;
            ViewData["CompletedTasks"] = completedTasks;

            return View();
        }

    }
}
