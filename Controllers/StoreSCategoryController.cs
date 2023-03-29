
using RMS.Interfaces;
using RMS.Models;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class StoreSCategoryController : Controller
    {
        private readonly IStoreSCategory _sCat;
        private readonly IStoreCategory _cat;

        public StoreSCategoryController(IStoreSCategory sCat, IStoreCategory Cat)
        {
            _sCat = sCat;
            _cat = Cat;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<StoreSCategory> items = _sCat.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var category = await _cat.GetAll();
            var categorydel = category.Select(v => new NameIdPair
            {
                Id = v.SCId,
                Name = v.SCName

            }).ToList();
            return new JsonResult(categorydel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(StoreSCategory model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.SSCId == 0)
                result = await _sCat.Create(model);
            else
            {
                var data = await _sCat.GetById(model.SSCId);
                if (data == null)
                    result = await _sCat.Create(model);
                else
                    result = await _sCat.Update(model);
            }

            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllSubCategoryClass()
        {
            var empRoaster = await _sCat.GetAll();
            return new JsonResult(empRoaster);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _sCat.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _sCat.GetById(id);
            return new JsonResult(response);
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsItemExists(int id)
        {
            bool isExists = _sCat.IsItemExists(id);
            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
