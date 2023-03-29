using System.Threading.Tasks;

namespace RMS.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermission _repo;
        public PermissionController(IPermission repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            var accCharts = await _repo.GetAll();
            return View(accCharts);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccPermission model)
        {
            if (ModelState.IsValid)
            {
                var result = await _repo.Create(model);
                if (result.Success)
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _repo.GetById(id);

            if (data == null)
            {
                ViewBag.ErrorMessage = $"Acc Chart with Id = {id} cannot be found";
                return View("NotFound");
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccPermission model)
        {
            if (ModelState.IsValid)
            {
                var result = await _repo.Update(model);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetById(id);

            if (data == null)
            {
                ViewBag.ErrorMessage = $"Acc Chart with Id = {id} cannot be found";
                return View("NotFound");
            }

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(AccPermission model)
        {
            var data = await _repo.Delete(model.Id);

            if (data.Success)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
