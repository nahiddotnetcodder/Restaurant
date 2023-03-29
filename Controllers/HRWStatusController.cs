using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class HRWStatusController : Controller
    {
        private readonly IHRWStatus _status;
        public HRWStatusController(IHRWStatus status)
        {
            _status = status;
        }
        public IActionResult Index(int pg = 1, int pageSize = 10)
        {
            PaginatedList<HRWStatus> items = _status.GetItems(pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(HRWStatus model)
        {
            string errMessage = "";
            try
            {
                _ = new ResponseStatus();
                ResponseStatus result;
                if (model.HRWSId == 0)
                    result = await _status.Create(model);
                else
                {
                    var data = await _status.GetById(model.HRWSId);
                    if (data == null)
                        result = await _status.Create(model);
                    else
                        result = await _status.Update(model);
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
            }
            return View(model);         
        }
        [HttpGet]
        public async Task<ActionResult> GetAllStatusClass()
        {
            var hrStatus = await _status.GetAll();
            return new JsonResult(hrStatus);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _status.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _status.GetById(id);
            return new JsonResult(response);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsNameExists(string name, int id)
        {
            bool isExists = _status.IsItemExists(name, id);
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
