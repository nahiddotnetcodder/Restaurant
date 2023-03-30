
using Microsoft.AspNetCore.Hosting;

namespace RMS.Controllers
{
    [Authorize]
    public class ResInfoController : Controller
    {

        private readonly IResInfo _Repo;
        private readonly IHostingEnvironment _webHostEnvironment;
        public ResInfoController(IResInfo repo, IHostingEnvironment webHostEnvironment) // here the repository will be passed by the dependency injection.
        {
            _Repo = repo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("RName");
            sortModel.AddColumn("RAddress");
            sortModel.AddColumn("RCity");
            sortModel.AddColumn("RPhone");
            sortModel.AddColumn("REmail");
            sortModel.AddColumn("RSCharge");
            sortModel.AddColumn("RTax");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<ResInfo> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        public IActionResult Create()
        {
            ResInfo item = new ResInfo();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(ResInfo item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.RName.Length < 2 || item.RName == null)
                    errMessage = "Restaurant Name Must be atleast 2 Characters";

                if (_Repo.IsItemExists(item.RName) == true)
                    errMessage = errMessage + " " + "Restaurant Name " + item.RName + " Exists Already";
                if (item.RCLogo != null)
                {
                    string filename = UploadImage(item);
                    item.RCLogoUrl = filename;
                }

                if (errMessage == "")
                {
                    item = _Repo.Create(item);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "" + item.RName + " Setup Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            ResInfo item = _Repo.GetItem(id);
            return View(item);
        }


        public IActionResult Edit(int id)
        {
            ResInfo item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(ResInfo item)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (item.RName.Length < 2 || item.RName == null)
                    errMessage = "Restaurant Name Must be atleast 2 Characters";

                if (_Repo.IsItemExists(item.RName, item.ResId) == true)
                    errMessage = errMessage +  item.RName + " Already Exists";
                if (item.RCLogo != null)
                {
                    string filename = UploadImage(item);
                    item.RCLogoUrl = filename;
                }
                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccessMessage"] = item.RName + ", Saved Successfully";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            ResInfo item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }
        public string UploadImage(ResInfo item)
        {
            string filename = null;
            if (item.RPhone != null)
            {
                string UploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + "_" + item.RCLogo.FileName;
                string filePath = Path.Combine(UploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    item.RCLogo.CopyTo(fileStream);

                }
            }
            return filename;
        }

    }
}
