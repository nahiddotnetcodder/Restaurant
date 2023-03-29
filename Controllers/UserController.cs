
using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly IRole _role;
        public UserController(IUser user, IRole role)
        {
            _user = user;
            _role = role;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _user.GetUsers();
            return View(users);
        }
        public async Task<IActionResult> Create()
        {
            var userRoles = await _role.GetAllAsTextValuePair();
            ViewData["UserRoles"] = userRoles;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User model)
        {
            if (ModelState.IsValid)
            {
                var result = await _user.CreateUser(model);
                if(result.Success)
                   return RedirectToAction("Index");
            }
            var userRoles = await _role.GetAllAsTextValuePair();
            ViewData["UserRoles"] = userRoles;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _user.GetUserById(id);

            var userRoles = await _role.GetAllAsTextValuePair();
            ViewData["UserRoles"] = userRoles;

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _user.UpdateUser(model);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
            }
            var userRoles = await _role.GetAllAsTextValuePair();
            ViewData["UserRoles"] = userRoles;
            return View(model);
        }
    }
}
