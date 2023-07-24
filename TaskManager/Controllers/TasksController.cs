using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskManagerDbContext _context;
        
        //changing structure
        private readonly ITaskRepository _taskRepository;
        
        public TasksController(ITaskRepository TaskRepository, TaskManagerDbContext context)
        {
            _context = context;
            _taskRepository = TaskRepository;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            var tasks = await _taskRepository.GetTasks();
            
            if(!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(tasks => tasks.Title.ToLower().Contains(searchString.ToLower()));
            }

            switch(sortOrder)
            {
                case "Title":
                    tasks = tasks.OrderBy(tasks => tasks.Title);
                    break;
                case "title_desc":
                    tasks = tasks.OrderByDescending(tasks => tasks.Title);
                    break;
                case "Date":
                    tasks = tasks.OrderBy(tasks => tasks.DueDate);
                    break;
                case "date_desc":
                    tasks = tasks.OrderByDescending(tasks => tasks.DueDate);
                    break;
                case "Status":
                    tasks = tasks.OrderBy(tasks => tasks.Status);
                    break;
                case "status_desc":
                    tasks = tasks.OrderByDescending(tasks => tasks.Status);
                    break;
                default:
                    tasks = tasks.OrderBy(tasks => tasks.Id);
                    break;
            }
            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _taskRepository.GetTaskById((int)id);
            //var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DueDate,isCompleted,Status")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                await _taskRepository.CreateTask(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _taskRepository.GetTasks == null)
            {
                return NotFound();
            }

            var task = await _taskRepository.GetTaskById((int)id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,DueDate,isCompleted, Status")] Models.Task task)
        {
/*            if (id != task.Id)
            {
                Console.WriteLine("here");
                return NotFound();
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                   await _taskRepository.UpdateTask(id, task);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null || _task.Tasks == null)
            //{
            //    return NotFound();
            //}

            var task = await _taskRepository.GetTaskById((int)id);
            //var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_taskRepository.GetTaskById(id) == null)
            {
                return Problem("Entity set 'TaskManagerDbContext.Tasks'  is null.");
            }
            var task = await _taskRepository.GetTaskById(id);
            if (task != null)
            {
                await _taskRepository.DeleteTask(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
