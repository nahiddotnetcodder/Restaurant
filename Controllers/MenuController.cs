using System.Threading.Tasks;
using static RMS.Models.ApplicationConstants;

namespace RMS.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenu _menu;
        public MenuController(IMenu menu)
        {
            _menu = menu;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(Menu model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.Id <= 0)
                result = await _menu.Create(model);
            else
                result = await _menu.Update(model);

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _menu.GetAll();
            var data = roles.Select(v => new NameIdPair
            {
                Id = v.Id,
                Name = v.Name
            }).ToList();
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var statuList = Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new NameIdPair
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return Json(statuList);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _menu.GetById(id);
            return Json(role);
        }
    }
}
