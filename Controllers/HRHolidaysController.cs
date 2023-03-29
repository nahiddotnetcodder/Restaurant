
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRHolidaysController : Controller
    {
        private readonly IHRHolidays _holidays;

        public HRHolidaysController(IHRHolidays holidays)
        {
            _holidays = holidays;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HRHolidays> items = _holidays.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpGet]
        public IActionResult GetInitData()
        {
            var holidayType = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            return new JsonResult(holidayType);
        }

        [HttpPost]
        public async Task<ActionResult> Save(HRHolidays model)
        {
            var result = await _holidays.Create(model);
            return Json(result);

        }
        [HttpPost]
        public async Task<ActionResult> Update(HRHolidays model)
        {
            var result = await _holidays.Update(model);
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllHolidayClass()
        {
            var holiday = await _holidays.GetAll();
            return new JsonResult(holiday);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _holidays.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _holidays.GetById(id);
            return new JsonResult(response);
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsItemExists(int id)
        {
            bool isExists = _holidays.IsItemExists(id);
            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
