using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class ResFoodTypeController : Controller
    {
        private readonly IResFoodType _foodType;

        public ResFoodTypeController(IResFoodType foodType)
        {
            _foodType = foodType;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<ResFoodType> items = _foodType.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(ResFoodType model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.RFTId == 0)
                result = await _foodType.Create(model);
            else
            {
                var data = await _foodType.GetById(model.RFTId);
                if (data == null)
                    result = await _foodType.Create(model);
                else
                    result = await _foodType.Update(model);
            }
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllFoodTypeClass()
        {
            var foodtype = await _foodType.GetAll();
            return new JsonResult(foodtype);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _foodType.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _foodType.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _foodType.IsItemExists(name, id);
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
