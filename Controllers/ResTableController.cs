using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRDesignationController : Controller
    {
        private readonly IHRDesignation _designation;
        public HRDesignationController(IHRDesignation designation)
        {
            _designation = designation;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HRDesignation> items = _designation.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(HRDesignation model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.HRDeId == 0)
                result = await _designation.Create(model);
            else
            {
                var data = await _designation.GetById(model.HRDeId);
                if (data == null)
                    result = await _designation.Create(model);
                else
                    result = await _designation.Update(model);
            }
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllDesigClass()
        {
            var hrdesig = await _designation.GetAll();
            return new JsonResult(hrdesig);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _designation.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _designation.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _designation.IsItemExists(name, id);
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
