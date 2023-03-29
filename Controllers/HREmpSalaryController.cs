using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;
using System.Runtime.ConstrainedExecution;

namespace RMS.Controllers
{
    [Authorize]
    public class HREmpSalaryController : Controller
    {
        private readonly IHREmpSalary _empSalary;
        private readonly IHREmpDetails _empDetails;

        public HREmpSalaryController(IHREmpSalary empSalary, IHREmpDetails empDetails ) // here the repository will be passed by the dependency injection.
        {
            _empSalary = empSalary;
            _empDetails = empDetails;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("HRSId");
            sortModel.AddColumn("HRSYear");
            sortModel.AddColumn("HRSMonth");
            sortModel.AddColumn("HREmpDetails");
            sortModel.AddColumn("HRSBasic");
            sortModel.AddColumn("HRSGTotal");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<HREmpSalary> resmenu = _empSalary.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(resmenu.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(resmenu);
        }

        public IActionResult Create()
        {
            HREmpSalary empsalary = new HREmpSalary();
            ViewBag.HREmpDetails = GetHREmpDetails();
            return View(empsalary);
        }
        [HttpPost]
        public IActionResult Create(HREmpSalary hrempsalary)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_empSalary.IsDNameExists(hrempsalary.HREDId) == true)
                    errMessage = "Employee Name Already Exists";
                if (errMessage == "")
                {
                    hrempsalary = _empSalary.Create(hrempsalary);
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
                ViewBag.HREmpDetails = GetHREmpDetails();
                return View(hrempsalary);
            }
            else
            {
                TempData["SuccessMessage"] = "Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id) //Read
        {
            HREmpSalary item = _empSalary.GetItem(id);
            ViewBag.HREmpDetails = GetHREmpDetails();
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            HREmpSalary item = _empSalary.GetItem(id);
            ViewBag.HREmpDetails = GetHREmpDetails();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(HREmpSalary hrempsalary)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_empSalary.IsDNameExists(hrempsalary.HREDId, hrempsalary.HRSId) == true)
                    errMessage = "Employee Name Already Exists";
                if (errMessage == "")
                {
                    hrempsalary = _empSalary.Edit(hrempsalary);
                    TempData["SuccessMessage"] ="Updated Successfully";
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
                return View(hrempsalary);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            HREmpSalary item = _empSalary.GetItem(id);
            ViewBag.HREmpDetails = GetHREmpDetails();
            _empSalary.Delete(item);
            TempData.Keep();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult Delete(HREmpSalary hrempsalary)
        //{
        //    try
        //    {
        //        hrempsalary = _empSalary.Delete(hrempsalary);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMessage = ex.Message;
        //        TempData["ErrorMessage"] = errMessage;
        //        ModelState.AddModelError("", errMessage);
        //        return View(hrempsalary);
        //    }
        //    int currentPage = 1;
        //    if (TempData["CurrentPage"] != null)
        //        currentPage = (int)TempData["CurrentPage"];
        //    TempData["SuccessMessage"] = "Deleted Successfully";
        //    return RedirectToAction(nameof(Index), new { pg = currentPage });
        //}
        private List<SelectListItem> GetHREmpDetails()
        {
            var hrempdetails = new List<SelectListItem>();
            PaginatedList<HREmpDetails> items = _empDetails.GetItems("HRDName", SortOrder.Ascending, "", 1, 1000);
            hrempdetails = items.Select(ut => new SelectListItem()
            {
                Value = ut.HREDId.ToString(),
                Text = ut.HREDEName
            }).ToList();
            //var defItem = new SelectListItem()
            //{
            //    Value = "",
            //    Text = "--Select Emp Name--"
            //};
            //hrempdetails.Insert(0, defItem);
            return hrempdetails;
        }
        
       
        [AcceptVerbs("Get","Post")]
        public JsonResult IsDNameExists(int id)
        {
            bool isExists = _empSalary.IsDNameExists(id);
            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
