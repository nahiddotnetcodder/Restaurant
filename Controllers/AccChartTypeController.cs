using System.Threading.Tasks;

namespace RMS.Controllers
{
    [Authorize]
    public class AccChartTypeController : Controller
    {
        private readonly IAccChartType _accRepo;
        private readonly IAccChart _accChart;
        public AccChartTypeController(IAccChartType accRepo, IAccChart accChart)
        {
            _accRepo = accRepo;
            _accChart = accChart;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var accTypes = await _accRepo.GetAll();
            var accTypesDD = accTypes.Select(v => new NameIdPair
            {
                Id = v.ACTId,
                Name = v.ACTId + "  " + v.ACTName

            }).ToList();

            var accCharts = await _accChart.GetAll();
            var accChartsDD = accCharts.Select(v => new NameIdPair
            {
                Id = v.ACCId,
                Name = v.ClassId + "  " + v.ACCName

            }).ToList();
            return new JsonResult(new { accTypesDD, accChartsDD });
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(AccChartType model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.ACTId == 0)
                result = await _accRepo.Create(model);
            else
            {
                var data = await _accRepo.GetById(model.ACTId);
                if (data == null)
                    result = await _accRepo.Create(model);
                else
                    result = await _accRepo.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllChartType()
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
