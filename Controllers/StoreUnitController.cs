
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class StoreUnitController : Controller
    {
        private readonly IStoreUnit _unit;
        public StoreUnitController(IStoreUnit unit)
        {
            _unit= unit;
        }
        public IActionResult Index( int pg = 1, int pageSize = 10)
        {
            PaginatedList<StoreUnit> resmenu = _unit.GetItems( pg, pageSize);
            var pager = new PagerModel(resmenu.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(resmenu);

        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(StoreUnit model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.SUId == 0)
                result = await _unit.Create(model);
            else
            {
                var data = await _unit.GetById(model.SUId);
                if (data == null)
                    result = await _unit.Create(model);
                else
                    result = await _unit.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllUnitClass()
        {
            var unitClass = await _unit.GetAll();
            return new JsonResult(unitClass);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unit.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _unit.GetById(id);
            return new JsonResult(response);
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsItemExists(string name, int id)
        {
            bool isExists = _unit.IsItemExists(name, id);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
