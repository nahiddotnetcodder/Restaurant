using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class AccFiscalYearController : Controller
    {
        private readonly IAccFiscalYear _repo;
        public AccFiscalYearController(IAccFiscalYear repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetInitData()
        {
            var yesNoDD = Enum.GetValues(typeof(YesNo)).Cast<YesNo>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            return new JsonResult(yesNoDD);
        }
        [HttpPost]
        public async Task<ActionResult> Save(AccFiscalYear model)
        {
            var result = await _repo.Create(model);
            return Json(result);
        }
        [HttpPost]
        public async Task<ActionResult> Update(AccFiscalYear model)
        {
            var result = await _repo.Update(model);
            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var data = await _repo.GetAll();
            return new JsonResult(data);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _repo.GetById(id);
            return new JsonResult(response);
        }
    }
}
