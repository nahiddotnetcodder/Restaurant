using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class AccBankAccountsController : Controller
    {
        private readonly IAccBankAccounts _repo;
        private readonly IAccChartMaster _repoMaster;
        private readonly IAccChartType _repoType;
        public AccBankAccountsController(IAccBankAccounts repo,IAccChartMaster repoMaster, IAccChartType repoType)
        {
            _repo = repo;
            _repoMaster = repoMaster;
            _repoType = repoType;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var ajId = Convert.ToInt32(id);
            var data = await _repo.GetById(ajId);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var chartMaster = await _repoMaster.GetAll();
            var chartMasterDD = chartMaster.Select(x => new NameIdAccountGroupPair
            {
                Id = x.ACMId,
                AccountGroupId = x.ACTId,
                Name = x.ACMAccCode + " " + x.ACMAccName,
                Code = x.ACMAccCode,
                Description = x.ACMAccName
            }).ToList();
            var chartType = await _repoType.GetAll();
            var chartTypeDD = chartType.Select(x => new NameIdPair
            {
                Id = x.ACTId,
                Name = x.ACTName
            }).ToList();

            var accType = Enum.GetValues(typeof(ABAAType)).Cast<ABAAType>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)
            }).ToList();
            return new JsonResult(new {chartMasterDD,chartTypeDD, accType });
        }
        [HttpPost]
        public async Task<ActionResult> Save(AccBankAccounts model)
        {
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
        public async Task<ActionResult> Update(AccBankAccounts model)
        {
            var result = await _repo.Update(model);
            return Json(result);
        }
    }
}
