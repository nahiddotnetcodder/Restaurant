using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRDepartmentController : Controller
    {
        private readonly IHRDepartment _department;
        public HRDepartmentController(IHRDepartment department)
        {
            _department = department;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HRDepartment> items = _department.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(HRDepartment model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.HRDId == 0)
                result = await _department.Create(model);
            else
            {
                var data = await _department.GetById(model.HRDId);
                if (data == null)
                    result = await _department.Create(model);
                else
                    result = await _department.Update(model);
            }
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllDeptClass()
        {
            var hrStatus = await _department.GetAll();
            return new JsonResult(hrStatus);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _department.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _department.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _department.IsItemExists(name, id);
            if (isExists)
            {
                return Json(data: false);
            }
            else
            {
                return Json(data: true);
            }
        }
    }
}
