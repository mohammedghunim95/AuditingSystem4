using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class RoleController : Controller
    {
        private readonly IBaseRepository<Role, int> _roleRepository;
        public RoleController(IBaseRepository<Role, int> roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if(userId != null) { 
                var role = await _roleRepository.ListAsync(
                       u => u.IsDeleted == false,
                       q => q.OrderBy(u => u.Id));

                return View(role);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _roleRepository.FindByAsync(id));
        }
    }
}
