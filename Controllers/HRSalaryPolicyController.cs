
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRSalaryPolicyController : Controller
    {
        private readonly IHRSalaryPolicy _policy;
        public HRSalaryPolicyController(IHRSalaryPolicy policy)
        {
            _policy = policy;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HRSalaryPolicy> items = _policy.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [HttpGet]
        public IActionResult GetInitData()
        {
            var adduc = Enum.GetValues(typeof(ADDUC)).Cast<ADDUC>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            
            var perNper = Enum.GetValues(typeof(PerNPer)).Cast<PerNPer>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            return new JsonResult(new { adduc, perNper });
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(HRSalaryPolicy model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.HRSPId == 0)
                result = await _policy.Create(model);
            else
            {
                var data = await _policy.GetById(model.HRSPId);
                if (data == null)
                    result = await _policy.Create(model);
                else
                    result = await _policy.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllSalaryPolicyClass()
        {
            var salaryPolicy = await _policy.GetAll();
            return new JsonResult(salaryPolicy);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _policy.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _policy.GetById(id);
            return new JsonResult(response);
        }
    }
}
