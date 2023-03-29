using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class RecManagerController : Controller
    {
        private readonly IRecManagerMaster _repo;
        private readonly IStoreIGen _igen;
        private readonly IStoreGoodsStock _stock;
        private readonly IResMenu _menu;
        private readonly IStoreUnit _unit;

        public RecManagerController(IRecManagerMaster repo,IStoreIGen igen, IStoreGoodsStock stock, IResMenu menu, IStoreUnit unit)
        {
            _repo = repo;
            _igen = igen;
            _stock = stock;
            _menu = menu;
            _unit = unit;
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
            var recMaster = await _menu.GetAll();
            var recMasterDD = recMaster.Select(x => new NameIdAccountGroupPair
            {
                Id = x.RMId,
                AccountGroupId = x.RMId,
                Name = x.RMItemCode + " " + x.RMItemName,
                Code = x.RMItemCode,
                Description = x.RMItemName
            }).ToList();
            var recType = await _menu.GetAll();
            var recTypeDD = recType.Select(x => new NameIdPair
            {
                Id = x.RMId,
                Name = x.RMItemName
            }).ToList();

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
            
            return new JsonResult(new { chartMasterDD, chartTypeDD, recMasterDD, recTypeDD });
        }
        [HttpPost]
        public async Task<ActionResult> Save(RMMaster model)
        {
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<RMDetails>>(model.Items);
                model.RMDetails = jsonItems;
            }
            var result = await _repo.Create(model);
            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllStockTable()
        {
            var restable = await _stock.GetAll();
            return new JsonResult(restable);
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
        public async Task<ActionResult> Update(RMMaster model)
        {
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<RMDetails>>(model.Items);
                model.RMDetails = jsonItems;
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

    }
}
