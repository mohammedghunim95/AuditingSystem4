using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class IndustryController : Controller
    {
        private readonly IBaseRepository<Industry, int> _industryRepository;
        public IndustryController(IBaseRepository<Industry, int> industryRepository)
        {
            _industryRepository = industryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _industryRepository.ListAsync(
                  i => i.IsDeleted == false,
                  q => q.OrderBy(u => u.ParentIndustryId),
                  i=>i.ParentIndustry);

            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            var industries = _industryRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ParentIndustryId = new SelectList(industries, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var industry = await _industryRepository.FindByAsync(id);

            var industries = _industryRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ParentIndustryId = new SelectList(industries, "Id", "Name", industry.ParentIndustryId);

            return View(industry);
        }
    }
}
