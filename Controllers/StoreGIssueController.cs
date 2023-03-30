using Newtonsoft.Json;
using NuGet.Protocol;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class StoreGIssueController : Controller
    {
        private readonly IStoreGIssueMaster _repo;
        private readonly IStoreIGen _igen;
        private readonly IHRDepartment _dept;
        private readonly IStoreGoodsStock _stock;
        private readonly IStoreCategory _cat;

        public StoreGIssueController(IStoreGIssueMaster repo, IStoreIGen igen, IHRDepartment dept, IStoreGoodsStock stock, IStoreCategory cat)
        {
            _repo = repo;
            _igen = igen;
            _dept = dept;
            _stock = stock;
            _cat = cat;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult masterDetails()
        {
            return View();
        }
        public IActionResult ItemDetails()
        {
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            var gimId = Convert.ToInt32(id);
            var data = await _repo.GetById(gimId);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {

            var chartMaster = await _stock.GetAll();
            var chartMasterDD = chartMaster.Select(x => new NameIdAccountGroupPair
            {
                Id = x.SGSId,
                AccountGroupId = x.SGSId,
                Name = x.ItemCode + " " + x.ItemName,
                Code = x.ItemCode,
                Description = x.ItemName,
                Unit = x.Unit,
                Price = x.SGSUPrice
            }).ToList();
            var chartType = await _stock.GetAll();
            var chartTypeDD = chartType.Select(x => new NameIdPair
            {
                Id = x.SGSId,
                Name = x.ItemName
            }).ToList();

            var deptName = await _dept.GetAll();
            var deptDetails = deptName.Select(v => new NameIdPair
            {
                Id = v.HRDId,
                Name = v.HRDName

            }).ToList();
            return new JsonResult(new { chartMasterDD, chartTypeDD, deptDetails });
        }
        [HttpPost]
        public async Task<ActionResult> Save(StoreGIssueMaster model)
        {
            if (model.Items == "[]")
            {
                TempData["ErrorMessage"] = "Item code was null!";
                return View();
            }
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<StoreGIssueDetails>>(model.Items);
                model.StoreGIssueDetails = jsonItems;
            }
            var result = await _repo.Create(model);
            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var data = await _repo.GetAll();
            return new JsonResult(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(int id)
        {
            var data = await _repo.GetById(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteItemById(int id)
        {
            var data = await _repo.DeleteItemById(id);
            return Json(data);
        }
        [HttpPost]
        public async Task<ActionResult> Update(StoreGIssueMaster model)
        {
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<StoreGIssueDetails>>(model.Items);
                model.StoreGIssueDetails = jsonItems;
            }
            var result = await _repo.Update(model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.Delete(id);
            return Json(data);
        }

        [HttpGet]
        public ActionResult DateValue()
        {
            var date = _repo.getdate();
            return new JsonResult(date);
        }
    }
}
