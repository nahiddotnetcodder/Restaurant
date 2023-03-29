using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;
using System.Runtime.ConstrainedExecution;

namespace RMS.Controllers
{
    [Authorize]
    public class HREmpAttController: Controller
    {
        private readonly IHREmpDetails _hrempdetailsRepo;
        private readonly IHREmpAtt _empAtt;
        private readonly RmsDbContext _context;

        public HREmpAttController(IHREmpDetails hrempdetailsRepo, IHREmpAtt empAtt,RmsDbContext context ) // here the repository will be passed by the dependency injection.
        {
            _hrempdetailsRepo = hrempdetailsRepo;
            _empAtt = empAtt;
            _context = context;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel(); 
            sortModel.AddColumn("HREAId");
            sortModel.AddColumn("HREmpDetails");
            sortModel.AddColumn("HREADate");
            sortModel.AddColumn("HREAInTime");
            sortModel.AddColumn("HREAOutTime");
            sortModel.AddColumn("TimeType");
            sortModel.AddColumn("HREATMinute");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<HREmpAtt> resmenu = _empAtt.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(resmenu.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(resmenu);
        }

        public IActionResult Create()
        {
            HREmpAtt empatt = new HREmpAtt();
            ViewBag.HREmpDetails = GetEmpDetails();
            return View(empatt);
        }
        [HttpPost]
        public IActionResult Create(HREmpAtt hreempatt)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_empAtt.IsENameExists(hreempatt.HREDId, hreempatt.HREADate) == true)
                    errMessage = "Employee Name Already Exists";
                if (errMessage == "")
                {
                    hreempatt = _empAtt.Create(hreempatt);
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
                ViewBag.HREmpDetails = GetEmpDetails();
                return View(hreempatt);
            }
            else
            {
                TempData["SuccessMessage"] = "Employee Profile Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id) //Read
        {
            HREmpAtt item = _empAtt.GetItem(id);
            ViewBag.HREmpDetails = GetEmpDetails();
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            HREmpAtt item = _empAtt.GetItem(id);
            ViewBag.HREmpDetails = GetEmpDetails();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(HREmpAtt hreempatt)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (_empAtt.IsENameExists(hreempatt.HREDId, hreempatt.HREADate) == true)
                    errMessage =  "Emp Name Already Exists";
                if (errMessage == "")
                {
                    hreempatt = _empAtt.Edit(hreempatt);
                    TempData["SuccessMessage"] = "Updated Successfully";
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
                ViewBag.HREmpDetails = GetEmpDetails();
                return View(hreempatt);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            HREmpAtt item = _empAtt.GetItem(id);
            ViewBag.HREmpDetails = GetEmpDetails();
            _empAtt.Delete(item);
            TempData.Keep();
            return RedirectToAction("Index");
        }
       
        private List<SelectListItem> GetEmpDetails()
        {
            var lstStatus = new List<SelectListItem>();
            PaginatedList<HREmpDetails> items = _hrempdetailsRepo.GetItems("HREDEName", SortOrder.Ascending, "", 1, 1000);
            lstStatus = items.Select(ut => new SelectListItem()
            {
                Value = ut.HREDId.ToString(),
                Text = ut.HREDEName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Employee Name----"
            };
            lstStatus.Insert(0, defItem);
            return lstStatus;
        }

        [AcceptVerbs("Get","Post")]
        public JsonResult IsENameExists(string name)
        {
            bool isExists = _empAtt.IsENameExists(name);
            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsENameExists(int id, DateTime date)
        {
            bool isExists = _empAtt.IsENameExists(id,date);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
    }
}
