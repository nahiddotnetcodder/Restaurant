
using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class AccChartController : Controller
    {
        private readonly IAccChart _accRepo;
        public AccChartController(IAccChart accRepo)
        {
            _accRepo = accRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetInitData()
        {
            var accTypes = Enum.GetValues(typeof(ACCCType)).Cast<ACCCType>().Select(v => new NameIdPair
            {
                Id = (int)v,
                Name = EnumUtility.GetDescriptionFromEnumValue(v)

            }).ToList();
            return new JsonResult(accTypes);
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(AccChartClass model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.ACCId == 0)
                result = await _accRepo.Create(model);
            else
            {
                var data = await _accRepo.GetById(model.ACCId);
                if (data == null)
                    result = await _accRepo.Create(model);
                else
                    result = await _accRepo.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllChartClass()
        {
            var accCharts = await _accRepo.GetAll();
            return new JsonResult(accCharts);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _accRepo.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _accRepo.GetById(id);
            return new JsonResult(response);
        }
    }
}
