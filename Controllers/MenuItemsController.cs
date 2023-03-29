using System.Threading.Tasks;
using static RMS.Models.ApplicationConstants;

namespace RMS.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItem _menuItems;
        private readonly IMenu _menu;
        public MenuItemController(IMenuItem menuItems, IMenu menu)
        {
            _menu = menu;
            _menuItems = menuItems;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetInitData()
        {
            var menus = await _menu.GetAll();
            var menusDD = menus.Select(v => new NameIdPair
            {
                Id = v.Id,
                Name = v.Name

            }).ToList();
            var statusDD = Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new NameIdPair
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return new JsonResult(new { menusDD, statusDD });
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(MenuItem model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            if (model.Id == 0)
                result = await _menuItems.Create(model);
            else
            {
                var data = await _menuItems.GetById(model.Id);
                if (data == null)
                    result = await _menuItems.Create(model);
                else
                    result = await _menuItems.Update(model);
            }

            return Json(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var menuItems = await _menuItems.GetAll();
            var menuItemsDD = menuItems.Select(v => new NameIdPair
            {
                Id = v.Id,
                Name = v.Name
            }).ToList();
            return new JsonResult(menuItemsDD);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _menuItems.Delete(id);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var response = await _menuItems.GetById(id);
            return new JsonResult(response);
        }
    }
}
