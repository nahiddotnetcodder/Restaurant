
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;

namespace RMS.Controllers
{
    [Authorize]
    public class StoreSuppliersController : Controller
    {
        private readonly IStoreSuppliers _Repo;
        private readonly RmsDbContext _context;

        public StoreSuppliersController(IStoreSuppliers repo, RmsDbContext context) // here the repository will be passed by the dependency injection.
        {
            _Repo = repo;
            _context = context;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("SSId");
            sortModel.AddColumn("SSName");
            sortModel.AddColumn("SSSName");
            sortModel.AddColumn("SSOAdd");
            sortModel.AddColumn("SSCPerson");
            sortModel.AddColumn("SSCNumber");
            sortModel.AddColumn("SSEmail");
            sortModel.AddColumn("SSBName");
            sortModel.AddColumn("SSGNotes");
            sortModel.AddColumn("AccMaster");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<StoreSuppliers> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        private void PopulateViewbags()
        {
            ViewBag.AccChartMaster = GetAccChartMaster();
        }

        public IActionResult Create()
        {
            StoreSuppliers item = new StoreSuppliers();
            PopulateViewbags();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(StoreSuppliers model)
        {
            bool bolret = false;
            string errMessage = "";
            PopulateViewbags();
            try
            {
                if (_Repo.IsEmpCodeExists(model.SSName, model.SSId) == true)
                    errMessage = "Supplier Name Already Exists";
                if (errMessage == "")
                {
                    model = _Repo.Create(model);
                    
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
               
                return View(model);
            }
            else
            {
                TempData["SuccessMessage"] = "Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            StoreSuppliers item = _Repo.GetItem(id);
            PopulateViewbags();
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            StoreSuppliers item = _Repo.GetItem(id);
            PopulateViewbags();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(StoreSuppliers item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.SSName.Length < 4 || item.SSName == null)
                    errMessage = "Supplier Name Must be atleast 4 Characters";
                if (_Repo.IsEmpCodeExists(item.SSName, item.SSId) == true)
                    errMessage = "Supplier Name Already Exists";
                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccessMessage"] = " Update Successfully";
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
            StoreSuppliers item = _Repo.GetItem(id);
            PopulateViewbags();
            TempData.Keep();
            _Repo.Delete(item);
            TempData["SuccessMessage"] = "Delete Successfully";
            return RedirectToAction(nameof(Index));
        }
        private List<SelectListItem> GetAccChartMaster()
        {
            var master = (from AccChartMaster in _context.AccChartMaster
                                select new SelectListItem()
                                {
                                    Text = AccChartMaster.ACMAccName,
                                    Value = AccChartMaster.ACMId.ToString(),
                                }).ToList();

            master.Insert(0, new SelectListItem()
            {
                Text = "----Select Account Master Type ----",
                Value = string.Empty
            });
            return master;
        }
    }
}
