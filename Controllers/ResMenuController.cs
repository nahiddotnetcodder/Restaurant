
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;

namespace RMS.Controllers
{
    [Authorize]
    public class ResMenuController : Controller
    {
        private readonly IResMenu _Repo;
        private readonly IResKitchenInfo _kitchenInfo;
        private readonly IResFoodType _foodType;

        public ResMenuController(IResMenu repo, IResKitchenInfo kitchenInfo, IResFoodType foodType) // here the repository will be passed by the dependency injection.
        {
            _Repo = repo;
            _kitchenInfo = kitchenInfo;
            _foodType = foodType;

        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("RMId");
            sortModel.AddColumn("ResKInfo");
            sortModel.AddColumn("ResFtype");
            sortModel.AddColumn("RMItemCode");
            sortModel.AddColumn("RMItemName");
            sortModel.AddColumn("RMUPrice");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<ResMenu> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }

        private void PopulateViewbags()
        {
            ViewBag.RKId = GetKitchenName();
            ViewBag.RFTId = GetResFoodType();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ResMenu item = new ResMenu();
            PopulateViewbags();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(ResMenu model)
        {
            bool bolret = false;
            string errMessage = "";
            PopulateViewbags();
            try
            {
                if (model.RMItemName.Length < 2 || model.RMItemName == null)
                    errMessage = "Item Name Must be atleast 2 Characters";

                if (_Repo.IsItemExists(model.RMItemCode, model.RMId) == true)
                    errMessage = "Item code Exists Already";
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
                TempData["SuccessMessage"] = "Item Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            ResMenu item = _Repo.GetItem(id);
            PopulateViewbags();
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            ResMenu item = _Repo.GetItem(id);
            PopulateViewbags();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(ResMenu item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.RMItemName.Length < 2 || item.RMItemName == null)
                    errMessage = "Item Name Must be atleast 2 Characters";
                if (_Repo.IsItemExists(item.RMItemCode, item.RMId) == true)
                    errMessage = "Item Code Already Exists";
                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccessMessage"] = "Item Update Successfully";
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
            ResMenu item = _Repo.GetItem(id);
            PopulateViewbags();
            TempData.Keep();
            _Repo.Delete(item);
            TempData["SuccessMessage"] = "Item Delete Successfully";
            return RedirectToAction(nameof(Index));
        }
        private List<SelectListItem> GetKitchenName()
        {
            var list = new List<SelectListItem>();
            PaginatedList<ResKitchenInfo> items = _kitchenInfo.GetItems(1, 1000);
            list = items.Select(ut => new SelectListItem()
            {
                Value = ut.RKId.ToString(),
                Text = ut.RKitchenName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Food Type----"
            };
            list.Insert(0, defItem);
            return list;
        }


        private List<SelectListItem> GetResFoodType()
        {
            var list = new List<SelectListItem>();
            PaginatedList<ResFoodType> items = _foodType.GetItems(1, 1000);
            list = items.Select(ut => new SelectListItem()
                {
                    Value = ut.RFTId.ToString(),
                    Text = ut.RFTName
                }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Food Type----"
            };
            list.Insert(0, defItem);
            return list;
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

    }
}
