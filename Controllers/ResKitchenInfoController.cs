using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class ResKitchenInfoController : Controller
    {
        private readonly IResKitchenInfo _resKitchen;

        public ResKitchenInfoController(IResKitchenInfo resKitchen)
        {
            _resKitchen = resKitchen;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<ResKitchenInfo> items = _resKitchen.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(ResKitchenInfo model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.RKId == 0)
                result = await _resKitchen.Create(model);
            else
            {
                var data = await _resKitchen.GetById(model.RKId);
                if (data == null)
                    result = await _resKitchen.Create(model);
                else
                    result = await _resKitchen.Update(model);
            }
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllKitchenClass()
        {
            var kitchentype = await _resKitchen.GetAll();
            return new JsonResult(kitchentype);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _resKitchen.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _resKitchen.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _resKitchen.IsItemExists(name, id);
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
