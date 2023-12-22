using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class YearController : Controller
    {
        private readonly IBaseRepository<Year, int> _yearRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        public YearController(
            IBaseRepository<Year, int> yearRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _yearRepository = yearRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            
            if(userId != null) { 
                var year = await _yearRepository.ListAsync(
                      u => u.IsDeleted == false,
                      q => q.OrderBy(u => u.Id),
                      u => u.Company, u => u.Department);

                return View(year);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Add()
        {
            var companies = await _companyRepository.ListAsync();
            var departments = await _departmentRepository.ListAsync();

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var year = await _yearRepository.FindByAsync(id);

            var companies = await _companyRepository.ListAsync();
            var departments = await _departmentRepository.ListAsync();

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name", year.CompanyId);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", year.DepartmentId);

            return View(year);
        }

    }
}
