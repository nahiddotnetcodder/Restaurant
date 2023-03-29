
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRLeavePolicyController : Controller
    {
        private readonly IHRLeavePolicy _policy;
        public HRLeavePolicyController(IHRLeavePolicy policy)
        {
            _policy = policy;
        }
        public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 10)
        {
            ViewBag.SearchText = SearchText;
            PaginatedList<HRLeavePolicy> resmenu = _policy.GetItems(pg, pageSize);
            var pager = new PagerModel(resmenu.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(resmenu);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(HRLeavePolicy model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.HRLPId == 0)
                result = await _policy.Create(model);
            else
            {
                var data = await _policy.GetById(model.HRLPId);
                if (data == null)
                    result = await _policy.Create(model);
                else
                    result = await _policy.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllLeavePolicyClass()
        {
            var leavepolicy = await _policy.GetAll();
            return new JsonResult(leavepolicy);
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
