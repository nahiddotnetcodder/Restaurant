using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class StoreCategoryController : Controller
    {
        private readonly IStoreCategory _category;
        public StoreCategoryController(IStoreCategory category)
        {
            _category = category;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<StoreCategory> items = _category.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(StoreCategory model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.SCId == 0)
                result = await _category.Create(model);
            else
            {
                var data = await _category.GetById(model.SCId);
                if (data == null)
                    result = await _category.Create(model);
                else
                    result = await _category.Update(model);
            }
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllCategoryClass()
        {
            var CategoryClass = await _category.GetAll();
            return new JsonResult(CategoryClass);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _category.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _category.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get","Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _category.IsItemExists(name, id);
            if(isExists)
            {
                return Json(data: false);
            }
            else
            {
                return Json(data: true);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsItemExists(string name, int id)
        {
            bool isExists = _category.IsItemExists(name, id);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
