
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;

namespace RMS.Controllers
{
    [Authorize]
    public class StoreIGenController : Controller
    {
        private readonly IStoreIGen _Repo;
        private readonly IStoreSCategory _storeSCategory;
        private readonly IStoreUnit _storeUnit;
        private readonly IStoreCategory _category;
        private readonly RmsDbContext _context;

        public StoreIGenController(IStoreIGen repo, IStoreSCategory storeSCategory, IStoreUnit storeUnit, IStoreCategory category,RmsDbContext context) // here the repository will be passed by the dependency injection.
        {
            _Repo = repo;
            _storeSCategory = storeSCategory;
            _storeUnit = storeUnit;
            _category = category;
            _context = context;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("SIGId");
            sortModel.AddColumn("StoreCategory");
            sortModel.AddColumn("StoreSCategory");
            sortModel.AddColumn("SIGItemCode");
            sortModel.AddColumn("SIGItemName");
            sortModel.AddColumn("StoreUnits");
            sortModel.AddColumn("SIGRLevel");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<StoreIGen> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        private void PopulateViewbags()
        {
            ViewBag.CategoryId = GetStoreCategory();
            ViewBag.StoreSCategory = GetStoreSCategory();
            ViewBag.StoreUnits = GetStoreUnits();
        }

        public IActionResult Create()
        {
            StoreIGen item = new StoreIGen();
            PopulateViewbags();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(StoreIGen storeIGen)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (storeIGen.SIGItemName.Length < 2 || storeIGen.SIGItemName == null)
                    errMessage = "Item Name Must be atleast 2 Characters";
                if (_Repo.IsItemExists(storeIGen.SIGItemName) == true)
                    errMessage = "Item Name Already Exists";

                if (_Repo.IsItemExists(storeIGen.SIGItemCode, storeIGen.SIGId) == true)
                    errMessage = "Item code Exists Already";
                if (errMessage == "")
                {
                    storeIGen = _Repo.Create(storeIGen);
                    
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
                PopulateViewbags();
                return View(storeIGen);
            }
            else
            {
                TempData["SuccessMessage"] = "Item Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            StoreIGen item = _Repo.GetItem(id);
            PopulateViewbags();
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            StoreIGen item = _Repo.GetItem(id);
            PopulateViewbags();
            ViewBag.StoreSCategory = GetStoreSCategory();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(StoreIGen item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.SIGItemName.Length < 2 || item.SIGItemName == null)
                    errMessage = "Item Name Must be atleast 2 Characters";
                if (_Repo.IsItemExists(item.SIGItemCode, item.SIGId) == true)
                    errMessage = "Item Code Already Exists";
                    
                if (errMessage == "")
                {
                    var exitcode = _context.StoreGoodsStock.Where(x => x.ItemCode == item.SIGItemCode).ToList();
                    if(exitcode == null)
                    {
                        item = _Repo.Edit(item);
                        TempData["SuccessMessage"] = "Item Update Successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Item already used!";
                    }
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
                PopulateViewbags();
                return View(item);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        public IActionResult Delete(int id)
        {
            StoreIGen item = _Repo.GetItem(id);
            PopulateViewbags();
            TempData.Keep();
            _Repo.Delete(item);
            TempData["SuccessMessage"] = "Item Delete Successfully";
            return RedirectToAction(nameof(Index));
        }
        private List<SelectListItem> GetStoreCategory()
        {
            var categorylist = new List<SelectListItem>();
            PaginatedList<StoreCategory> items = _category.GetItems(1, 1000);
            categorylist = items.Select(ut => new SelectListItem()
            {
                Value = ut.SCId.ToString(),
                Text = ut.SCName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Category----"
            };
            categorylist.Insert(0, defItem);
            return categorylist;
        }


        private List<SelectListItem> GetStoreSCategory(int SCId = 1)
        {
            var sscategory = new List<SelectListItem>();
            PaginatedList<StoreSCategory> items = _storeSCategory.GetItems( 1, 1000);
            sscategory = items.Where(c=>c.SCId == SCId)
                .OrderBy(n=>n.SSCName)
                .Select(ut => new SelectListItem()
            {
                Value = ut.SSCId.ToString(),
                Text = ut.SSCName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Sub Category Name----"
            };
            sscategory.Insert(0, defItem);
            return sscategory;
        }

        private List<SelectListItem> GetStoreUnits()
        {
            var unitlist = new List<SelectListItem>();
            PaginatedList<StoreUnit> items = _storeUnit.GetItems(1, 1000);
            unitlist = items.Select(ut => new SelectListItem()
                {
                    Value = ut.SUId.ToString(),
                    Text = ut.SUName
                }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Unit----"
            };
            unitlist.Insert(0, defItem);
            return unitlist;
        }

  
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsItemExists(string code, int id)
        {
            bool isExists = _Repo.IsItemExists(code,id);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }

        [HttpGet]
        public JsonResult GetCatBySub(int SCId)
        {
            List<SelectListItem> subCat = GetStoreSCategory(SCId);
            return Json(subCat);
        }

    }
}
