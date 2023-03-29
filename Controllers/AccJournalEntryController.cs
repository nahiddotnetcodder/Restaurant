using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class AccJournalEntryController : Controller
    {
        private readonly IAccJournalEntry _repo;
        private readonly IAccFiscalYear _repoFiscalYear;
        private readonly IAccChartMaster _repoMaster;
        private readonly IAccChartType _repoType;
        public AccJournalEntryController(IAccJournalEntry repo,
            IAccChartMaster repoMaster,
            IAccFiscalYear repoFiscalYear,
            IAccChartType repoType)
        {
            _repo = repo;
            _repoMaster = repoMaster;
            _repoFiscalYear = repoFiscalYear;
            _repoType = repoType;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Journals()
        {
            return View();
        }
        public IActionResult JournalDetails()
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
            var refPrefixNo = await _repo.GetRefPrefixNo();
            var refNo = refPrefixNo + "/" + DateTime.Now.Year.ToString();
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
            var fiscalYear = await _repoFiscalYear.GetAll();
            var lastFiscalYear = fiscalYear?.Where(x => x.AFYClosed == (int)YesNo.No).OrderBy(x => x.AFYId).LastOrDefault();
            var fiscalBeginYear = lastFiscalYear?.AFYBeginDate;
            var fiscalEndYear = lastFiscalYear?.AFYEndDate;
            return new JsonResult(new { fiscalBeginYear, fiscalEndYear, refPrefixNo, refNo, chartMasterDD, chartTypeDD });
        }
        [HttpPost]
        public async Task<ActionResult> Save(AccJournal model)
        {
            if(model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<AccGlTrans>>(model.Items);
                model.AccGlTrans = jsonItems;
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
        public async Task<ActionResult> Update(AccJournal model)
        {
            if (model.Items != null)
            {
                var jsonItems = JsonConvert.DeserializeObject<List<AccGlTrans>>(model.Items);
                model.AccGlTrans = jsonItems;
            }
            var result = await _repo.Update(model);
            return Json(result);
        }
    }
}
