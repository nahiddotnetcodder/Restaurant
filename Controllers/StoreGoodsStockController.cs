using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;
using System.Runtime.ConstrainedExecution;

namespace RMS.Controllers
{
    [Authorize]
    public class StoreGoodsStockController : Controller
    {
        private readonly IStoreGoodsStock _storeGoods;
        private readonly IStoreIGen _storeigen;

        public StoreGoodsStockController(IStoreGoodsStock storeGoods, IStoreIGen storeigen) // here the repository will be passed by the dependency injection.
        {
            _storeGoods = storeGoods;
            _storeigen = storeigen;
        } 
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("ItemCode");
            sortModel.AddColumn("ItemName");
            sortModel.AddColumn("Unit"); 
            sortModel.AddColumn("SGSQty"); 
            sortModel.AddColumn("SGSUPrice");
            sortModel.AddColumn("SGSTPrice");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<StoreGoodsStock> resmenu = _storeGoods.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(resmenu.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(resmenu);
        }

        public IActionResult Create()
        {
            StoreGoodsStock empdetails = new StoreGoodsStock();
            ViewBag.storeIGen = GetStoreIGen();
            return View(empdetails);
        }
        [HttpPost]
        public IActionResult Create(StoreGoodsStock storeGoodsStock)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
               
                if (errMessage == "")
                {
                    storeGoodsStock = _storeGoods.Create(storeGoodsStock);
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
                ViewBag.storeIGen = GetStoreIGen();
                return View(storeGoodsStock);
            }
            else
            {
                TempData["SuccessMessage"] = "Employee Profile " + storeGoodsStock.SGSId + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id) //Read
        {
            StoreGoodsStock item = _storeGoods.GetItem(id);
            ViewBag.storeIGen = GetStoreIGen();
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            StoreGoodsStock item = _storeGoods.GetItem(id);
            ViewBag.storeIGen = GetStoreIGen();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(StoreGoodsStock storeGoodsStock)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (errMessage == "")
                {
                    storeGoodsStock = _storeGoods.Edit(storeGoodsStock);
                    TempData["SuccessMessage"] = storeGoodsStock.SGSId + ",  Employee Profile Updated Successfully";
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
                return View(storeGoodsStock);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        public IActionResult Delete(int id)
        {
            StoreGoodsStock item = _storeGoods.GetItem(id);
            ViewBag.storeIGen = GetStoreIGen();
            TempData.Keep();
            return View(item);
        }

        [HttpPost]
        public IActionResult Delete(StoreGoodsStock storeGoodsStock)
        {
            try
            {
                storeGoodsStock = _storeGoods.Delete(storeGoodsStock);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(storeGoodsStock);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];
            TempData["SuccessMessage"] = storeGoodsStock.SGSId + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        private List<SelectListItem> GetStoreIGen()
        {
            var storeigen = new List<SelectListItem>();
            PaginatedList<StoreIGen> items = _storeigen.GetItems("SIGItemName", SortOrder.Ascending, "", 1, 1000);
            storeigen = items.Select(ut => new SelectListItem()
            {
                Value = ut.SIGId.ToString(),
                Text = ut.SIGItemName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Item Name----"
            };
            storeigen.Insert(0, defItem);
            return storeigen;
        }
    }
}
