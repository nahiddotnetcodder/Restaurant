
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRWeekendController : Controller
    {
        private readonly IHRWeekend _weekend;
        public HRWeekendController(IHRWeekend weekend)
        {
            _weekend = weekend;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HRWeekend> items = _weekend.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [HttpGet]
        public IActionResult GetInitData()
        {
            var accTypes = Enum.GetValues(typeof(Weekday)).Cast<Weekday>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            return new JsonResult(accTypes);
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(HRWeekend model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.HRWId == 0)
                result = await _weekend.Create(model);
            else
            {
                var data = await _weekend.GetById(model.HRWId);
                if (data == null)
                    result = await _weekend.Create(model);
                else
                    result = await _weekend.Update(model);
            }

            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllWeekdayClass()
        {
            var weekdays = await _weekend.GetAll();
            return new JsonResult(weekdays);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _weekend.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _weekend.GetById(id);
            return new JsonResult(response);
        }
    }
}
