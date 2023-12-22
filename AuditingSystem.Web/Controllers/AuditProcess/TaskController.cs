using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class TaskController : Controller
    {
        private readonly IBaseRepository<Tasks, int> _tasksRepository;
        private readonly IBaseRepository<Objective, int> _objectiveRepository;
        public TaskController(
            IBaseRepository<Tasks, int> tasksRepository,
            IBaseRepository<Objective, int> objectiveRepository)
        {
            _tasksRepository = tasksRepository;
            _objectiveRepository = objectiveRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _tasksRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Id),
                  c => c.Objective);

            return View(tasks);
        }

        public async Task<IActionResult> Add()
        {
            var objective = _objectiveRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var task = await _tasksRepository.FindByAsync(id);

            var objective = _objectiveRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name", task.ObjectiveId);

            return View(task);
        }
    }
}
