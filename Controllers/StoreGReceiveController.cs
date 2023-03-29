using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class StoreGReceiveController : Controller
    {
        private readonly IStoreGReceiveMaster _repo;
        private readonly IStoreIGen _igen;
        private readonly IStoreSuppliers _suppliers;
        private readonly IStoreCategory _cat;

        public StoreGReceiveController(IStoreGReceiveMaster repo,IStoreIGen igen, IStoreSuppliers suppliers, IStoreCategory cat)
        {
            _repo = repo;
            _igen = igen;
            _suppliers = suppliers;
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
            var grmId = Convert.ToInt32(id);
            var data = await _repo.GetById(grmId);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var chartMaster = await _igen.GetAll();
            var chartMasterDD = chartMaster.Select(x => new NameIdAccountGroupPair
            {
                Id = x.SIGId,
                AccountGroupId = x.SCId,
                Name = x.SIGItemCode + " " + x.SIGItemName,
                Code = x.SIGItemCode,
                Description = x.SIGItemName,
                Unit = x.StoreUnits.SUName
            }).ToList();
            var chartType = await _cat.GetAll();
            var chartTypeDD = chartType.Select(x => new NameIdPair
            {
                Id = x.SCId,
                Name = x.SCName
            }).ToList();
            
            var empName = await _suppliers.GetAll();
            var empDetails = empName.Select(v => new NameIdPair
            {
                Id = v.SSId,
                Name = v.SSName

            }).ToList();
            return new JsonResult(new { chartMasterDD, chartTypeDD, empDetails });
        }
        [HttpPost]
        public async Task<ActionResult> Save(StoreGReceiveMaster model)
        {
            if (model.Items == "[]")
            {
                TempData["ErrorMessage"] = "Item code was null!";
                return View();
            }
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<StoreGReceiveDetails>>(model.Items);
                model.StoreGReceiveDetails = jsonItems;
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
        public async Task<ActionResult> Update(StoreGReceiveMaster model)
        {
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<StoreGReceiveDetails>>(model.Items);
                model.StoreGReceiveDetails = jsonItems;
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
        public  ActionResult DateValue()
        {
            var date =  _repo.getdate();
            return new JsonResult(date);
        }
    }
}
