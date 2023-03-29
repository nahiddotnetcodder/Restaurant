using Newtonsoft.Json;
using System.Threading.Tasks;
using static RMS.Models.ApplicationConstants;

namespace RMS.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRole _role;
        private readonly IMenu _menu;
        private readonly IMenuItem _menuItem;
        public RoleController(IRole role,IMenu menu,IMenuItem menuItem)
        {
            _role = role;
            _menu = menu;
            _menuItem = menuItem;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SaveOrUpdate(ApplicationRole model)
        {
            _ = new ResponseStatus();
            ResponseStatus result;
            var selectedMenuItems = JsonConvert.DeserializeObject<List<MenuItem>>(model.SelectedMenuItems);
            if (string.IsNullOrEmpty(model.Id))
                result = await _role.CreateRole(model, selectedMenuItems);
            else
                result = await _role.UpdateRole(model, selectedMenuItems);

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _role.GetAllAsTextValuePair();
            return Json(roles);
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
        public async Task<IActionResult> GetAssignedMenus(string roleId)
        {
            var allMenuPermissions = new List<MenuPermissions>();
            if (roleId!=null)
                allMenuPermissions = await _role.GetAllMenuPermissions(roleId);
            var menus = await _menu.GetAll();
            var menuItems = await _menuItem.GetAll();
            foreach (var menuItem in menuItems)
            {
                var permissionExists = allMenuPermissions.Where(x => x.RoleId == roleId && x.MenuId == menuItem.MenuId && x.MenuItemId == menuItem.Id).FirstOrDefault();
                if(permissionExists!=null)
                {
                    menuItem.IsSelected = permissionExists.IsActive;
                }
            }
            return new JsonResult(new { menus, menuItems });
        }
        [HttpGet]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _role.GetRoleById(id);
            return Json(role);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserAssignedMenus()
        {
            var currentUserMenus = new List<Menu>();
            var currentUserMenuItems = new List<MenuItem>();

            var menus = await _menu.GetAll();
            var menuItems = await _menuItem.GetAll();
            var assignMenus = await _role.GetUserAssignedMenus();

            foreach (var item in menus)
            {
                var permissionExists = assignMenus.Where(x => x.MenuId == item.Id).FirstOrDefault();
                if (permissionExists != null)
                {
                    currentUserMenus.Add(item);
                }
            }
            foreach (var menuItem in menuItems)
            {
                var permissionExists = assignMenus.Where(x => x.MenuId == menuItem.MenuId && x.MenuItemId == menuItem.Id).FirstOrDefault();
                if (permissionExists != null)
                {
                    currentUserMenuItems.Add(menuItem);
                }
            }
            string controllerName = string.Empty;
            string htmlMenu = "<ul class='navbar-nav flex-grow-1'>";
            htmlMenu += "<li class='nav-item'><a class='nav-link text-white' style='cursor:pointer' href='/Home/Index'>Home</a></li>";
            foreach(var item in currentUserMenus)
            {
                htmlMenu += "<li class='dropdown nav-item pr-2'>";
                htmlMenu += "<a class='nav-link dropdown-toggle text-white' style='cursor:pointer' data-toggle='dropdown' aria-haspopup='true' aria-expanded='true'>" + item.Name+"</a>";
                htmlMenu += "<ul class='dropdown-menu' style='background: linear-gradient(to right,#28340B,#197696);'>";
                foreach (var menuItem in currentUserMenuItems)
                {
                    if(menuItem.MenuId == item.Id)
                    {
                        controllerName = GetControllerName(menuItem.Name);
                        htmlMenu += "<li class='nav-item'>";
                        htmlMenu += "<a class='nav-link text-white' style='cursor:pointer' href='/"+ controllerName + "/Index'>" + menuItem.Name+ "</a>";
                        htmlMenu += "</li>";
                    }
                }
                htmlMenu += "</li>";
                htmlMenu += "</ul>";
            }
            htmlMenu += "<li class='nav-item'><a class='nav-link text-white' style='cursor:pointer' href='/Home/Privacy'>Privacy</a></li></ul>";
            return new JsonResult(htmlMenu);
        }
        public string GetControllerName(string menuItem)
        {
            string controllerName = string.Empty;
            switch(menuItem)
            {
                case MenuItemsConstant.Users:
                    controllerName = ControllerConstant.User;
                    break;
                case MenuItemsConstant.Roles:
                    controllerName = ControllerConstant.Role;
                    break;
                case MenuItemsConstant.Menus:
                    controllerName = ControllerConstant.Menu;
                    break;
                case MenuItemsConstant.Menu_Items:
                    controllerName = ControllerConstant.MenuItem;
                    break;
                case MenuItemsConstant.Permissions:
                    controllerName = ControllerConstant.Permission;
                    break;
                case MenuItemsConstant.Chart_Class:
                    controllerName = ControllerConstant.AccChart;
                    break;
                case MenuItemsConstant.Chart_Type:
                    controllerName = ControllerConstant.AccChartType;
                    break;
                case MenuItemsConstant.Chart_Master:
                    controllerName = ControllerConstant.AccChartMaster;
                    break;
                case MenuItemsConstant.Fiscal_Year:
                    controllerName = ControllerConstant.AccFiscalYear;
                    break;
                case MenuItemsConstant.Journal_Entry:
                    controllerName = ControllerConstant.AccJournalEntry;
                    break;
                case MenuItemsConstant.Journal_Lists:
                    controllerName = ControllerConstant.AccJournalEntry;
                    break;
                case MenuItemsConstant.Restaurant_Setup:
                    controllerName = ControllerConstant.ResInfo;
                    break;
                case MenuItemsConstant.Kitchen_Setup:
                    controllerName = ControllerConstant.ResKitchenInfo;
                    break;
                case MenuItemsConstant.Food_Type_Setup:
                    controllerName = ControllerConstant.ResFoodType;
                    break;
                case MenuItemsConstant.Food_Menu_Setup:
                    controllerName = ControllerConstant.ResMenu;
                    break;
                case MenuItemsConstant.Date_Setup:
                    controllerName = ControllerConstant.ResDClose;
                    break;
                case MenuItemsConstant.Department_Setup:
                    controllerName = ControllerConstant.HRDepartment;
                    break;
                case MenuItemsConstant.Designation_Setup:
                    controllerName = ControllerConstant.HRDesignation;
                    break;
                case MenuItemsConstant.Work_Status_Setup:
                    controllerName = ControllerConstant.HRWStatus;
                    break;
                case MenuItemsConstant.Employee_Details_Setup:
                    controllerName = ControllerConstant.HREmpDetails;
                    break;
                case MenuItemsConstant.Billing:
                    controllerName = ControllerConstant.ResSales;
                    break;
                case MenuItemsConstant.Units:
                    controllerName = ControllerConstant.Unit;
                    break;
                case MenuItemsConstant.Products:
                    controllerName = ControllerConstant.Product;
                    break;
                case MenuItemsConstant.Categories:
                    controllerName = ControllerConstant.Category;
                    break;
                case MenuItemsConstant.Brands:
                    controllerName = ControllerConstant.Brand;
                    break;
                case MenuItemsConstant.Product_Profiles:
                    controllerName = ControllerConstant.ProductProfile;
                    break;
                case MenuItemsConstant.Product_Groups:
                    controllerName = ControllerConstant.ProductGroup;
                    break;
                case MenuItemsConstant.Suppliers:
                    controllerName = ControllerConstant.Supplier;
                    break;
                case MenuItemsConstant.Currencies:
                    controllerName = ControllerConstant.Currency;
                    break;
                default:
                    break;
            }
            return controllerName;
        }
    }
}
