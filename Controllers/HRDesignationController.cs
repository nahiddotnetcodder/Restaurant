using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class ResTableController : Controller
    {
        private readonly IResTable _resTable;
        public ResTableController(IResTable resTable)
        {
            _resTable = resTable;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<ResTable> items = _resTable.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(ResTable model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.RTId == 0)
                result = await _resTable.Create(model);
            else
            {
                var data = await _resTable.GetById(model.RTId);
                if (data == null)
                    result = await _resTable.Create(model);
                else
                    result = await _resTable.Update(model);
            }
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllResTableClass()
        {
            var restable = await _resTable.GetAll();
            return new JsonResult(restable);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _resTable.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _resTable.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _resTable.IsItemExists(name, id);
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
