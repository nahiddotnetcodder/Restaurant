
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class AccChartMasterController : Controller
    {
        private readonly IAccChartMaster _repo;
        private readonly IAccChartType _repoType;
        public AccChartMasterController(IAccChartMaster repo, IAccChartType repoType)
        {
            _repo = repo;
            _repoType = repoType;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAccountDetails()
        {
            var accounts = await _repo.GetAll();
            return new JsonResult( accounts );
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAccountDetailsIncludeInactive()
        {
            var accounts = await _repo.GetAllIncludeInactive();
            return new JsonResult(accounts);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAccount()
        {
            var accounts = await _repo.GetAll();
            var accountsDD = accounts.Select(v => new NameIdAccountGroupPair
            {
                Id = v.ACMId,
                AccountGroupId = v.ACTId,
                Name = v.ACMAccCode + " " + v.ACMAccName

            }).ToList();
            var accChartTypes = await _repoType.GetAll();
            var accChartTypesDD = accChartTypes.Select(x => new NameIdPair
            {
                Id = x.ACTId,
                Name = x.ACTClassId + " " + x.ACTName
            });
            return new JsonResult(new { accounts, accountsDD, accChartTypesDD });
        }
        [HttpGet]
        public async Task<ActionResult> GetAllIncludeInactive()
        {
            var accounts = await _repo.GetAllIncludeInactive();
            var accountsDD = accounts.Select(v => new NameIdAccountGroupPair
            {
                Id = v.ACMId,
                AccountGroupId = v.ACTId,
                Name = v.ACMAccCode + " " + v.ACMAccName

            }).ToList();
            var accChartTypes = await _repoType.GetAll();
            var accChartTypesDD = accChartTypes.Select(x => new NameIdPair
            {
                Id = x.ACTId,
                Name = x.ACTClassId + " " + x.ACTName
            });
            return new JsonResult(new { accountsDD, accChartTypesDD });
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var statusDD = Enum.GetValues(typeof(ACMAI)).Cast<ACMAI>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            var accChartTypes = await _repoType.GetAll();
            var accChartTypesDD = accChartTypes.Select(x => new NameIdPair
            {
                Id = x.ACTId,
                Name = x.ACTClassId + " " + x.ACTName
            });
            return new JsonResult(new { statusDD, accChartTypesDD });
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(AccChartMaster model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.ACMId == 0)
                result = await _repo.Create(model);
            else
            {
                var data = await _repo.GetById(model.ACMId);
                if (data == null)
                    result = await _repo.Create(model);
                else
                    result = await _repo.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _repo.GetById(id);
            return new JsonResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(AccChartMaster model)
        {
            var data = await _repo.Delete(model.ACMId);
            return Json(data);
        }
    }
}
