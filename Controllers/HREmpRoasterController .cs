
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class HREmpRoasterController : Controller
    {
        private readonly IHREmpRoaster _roaster;
        private readonly IHREmpDetails _details;

        public HREmpRoasterController(IHREmpRoaster roaster, IHREmpDetails details)
        {
            _roaster = roaster;
            _details = details;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HREmpRoaster> items = _roaster.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var shiftType = Enum.GetValues(typeof(ShiftType)).Cast<ShiftType>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)
            }).ToList();

            var empName = await _details.GetAll();
            var empDetails = empName.Select(v => new NameIdPair
            {
                Id = v.HREDId,
                Name = v.HREDEName

            }).ToList();
            return new JsonResult(new { shiftType, empDetails });
        }

        [HttpPost]
        public async Task<ActionResult> Save(HREmpRoaster model)
        {
            var result = await _roaster.Create(model);
            return Json(result);

        }
        [HttpPost]
        public async Task<ActionResult> Update(HREmpRoaster model)
        {
            var result = await _roaster.Update(model);
            return Json(result);

        }
        [HttpGet]
        public async Task<ActionResult> GetAllEmpRoasterClass()
        {
            var empRoaster = await _roaster.GetAll();
            return new JsonResult(empRoaster);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _roaster.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _roaster.GetById(id);
            return new JsonResult(response);
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsItemExists(int id)
        {
            bool isExists = _roaster.IsItemExists(id);
            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
