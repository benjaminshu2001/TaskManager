using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class TaskManagerUserController : Controller
    {

        private readonly ITaskManagerUserRepository _taskManagerUserRepository;

        public TaskManagerUserController(ITaskManagerUserRepository taskManagerUserRepository)
        {
            _taskManagerUserRepository = taskManagerUserRepository;
        }

        // GET: TaskManagerUserController
        public async Task<ActionResult> Index()
        {
            var users = await _taskManagerUserRepository.GetUsers();

            return View(users);
        }

        // GET: TaskManagerUserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaskManagerUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskManagerUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskManagerUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskManagerUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskManagerUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskManagerUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
