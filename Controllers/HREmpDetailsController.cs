using Microsoft.AspNetCore.Hosting;
using RMS.Interfaces;
using RMS.Models;
using RMS.Repositories;
using System.Runtime.ConstrainedExecution;

namespace RMS.Controllers
{
    [Authorize]
    public class HREmpDetailsController : Controller
    {
        private readonly IHREmpDetails _hrempdetailsRepo;
        private readonly IHRDepartment _hrdepartmentRepo;
        private readonly IHRDesignation _hrdesignationRepo;
        private readonly IHRWStatus _hrwstatusRepo;
        private readonly IHostingEnvironment _webHostEnvironment;

        public HREmpDetailsController(IHREmpDetails hrempdetailsRepo, IHRDepartment hrdepartmentRepo, IHRDesignation hrdesignationRepo, IHRWStatus hrwstatusRepo,IHostingEnvironment hostingEnvironment) // here the repository will be passed by the dependency injection.
        {
            _hrempdetailsRepo = hrempdetailsRepo;
            _hrdepartmentRepo = hrdepartmentRepo;
            _hrdesignationRepo = hrdesignationRepo;
            _hrwstatusRepo = hrwstatusRepo;
            _webHostEnvironment = hostingEnvironment;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("HREDEId");
            sortModel.AddColumn("HREDEName");
            sortModel.AddColumn("HREDCont");
            sortModel.AddColumn("HDEDBGroup");
            sortModel.AddColumn("HRDesig");
            sortModel.AddColumn("HRDepart");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<HREmpDetails> resmenu = _hrempdetailsRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(resmenu.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(resmenu);
        }
        private void PopulateViewbags()
        {
            ViewBag.HRDepartment = GetHRDepartments();
            ViewBag.HRDesignation = GetHRDesignations();
            ViewBag.HRWStatus = GetHRWStatus();
        }
        public IActionResult Create()
        {
            HREmpDetails empdetails = new HREmpDetails();
            empdetails.HREDEId = _hrempdetailsRepo.GetEmpID(); //Get Auto Genarate Employee ID
            PopulateViewbags();
            return View(empdetails);
        }

        [HttpPost]
        public IActionResult Create(HREmpDetails empdetails)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (empdetails.HREDEName.Length < 4 || empdetails.HREDEName == null)
                    errMessage = "Employee Name Must be atleast 4 Characters";
                if (_hrempdetailsRepo.IsEmpCodeExists(empdetails.HREDEName, empdetails.HREDId) == true)
                    errMessage = "Employee Name Exists Already";
                if (empdetails.HREDPPhoto != null)
                {
                    string filename = UploadImage(empdetails);
                    empdetails.HREDPUrl = filename;
                }
                if (errMessage == "")
                {
                    empdetails.HREDEId = _hrempdetailsRepo.GetEmpID();
                    empdetails = _hrempdetailsRepo.Create(empdetails);
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
                return View(empdetails);
            }
            else
            {
                TempData["SuccessMessage"] = "Employee Profile Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int id) //Read
        {
            HREmpDetails item = _hrempdetailsRepo.GetItem(id);
            PopulateViewbags();
            return View(item);
        }
        public IActionResult Edit(int id)
        {
            HREmpDetails item = _hrempdetailsRepo.GetItem(id);
            PopulateViewbags();
            TempData.Keep();
            return View(item);
        }
        [HttpPost]
        public IActionResult Edit(HREmpDetails empdetails)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (empdetails.HREDEName.Length < 4 || empdetails.HREDEName == null)
                    errMessage = "Employee Name Must be atleast 4 Characters";
                if (_hrempdetailsRepo.IsEmpCodeExists(empdetails.HREDEName, empdetails.HREDId) == true)
                    errMessage = "Employee Name Exists Already";
                if (empdetails.HREDPPhoto != null)
                {
                    string filename = UploadImage(empdetails);
                    empdetails.HREDPUrl = filename;
                }
                if (errMessage == "")
                {
                    //empdetails.HREDEId = _hrempdetailsRepo.GetEmpID();
                    empdetails = _hrempdetailsRepo.Edit(empdetails);
                    TempData["SuccessMessage"] =  "Employee Profile Updated Successfully";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
                Console.WriteLine(errMessage);
            }
            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(empdetails);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            HREmpDetails item = _hrempdetailsRepo.GetItem(id);
            _hrempdetailsRepo.Delete(item);
            TempData["SuccessMessage"] = "Item Delete Successfully";
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetHRDepartments()
        {
            var lstDepartment = new List<SelectListItem>();
            PaginatedList<HRDepartment> items = _hrdepartmentRepo.GetItems( 1, 1000);
            lstDepartment = items.Select(ut => new SelectListItem()
            {
                Value = ut.HRDId.ToString(),
                Text = ut.HRDName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Department----"
            };
            lstDepartment.Insert(0, defItem);
            return lstDepartment;
        }
        private List<SelectListItem> GetHRDesignations()
        {
            var lstDesignation = new List<SelectListItem>();
            PaginatedList<HRDesignation> items = _hrdesignationRepo.GetItems(1, 1000);
            lstDesignation = items.Select(ut => new SelectListItem()
            {
                Value = ut.HRDeId.ToString(),
                Text = ut.HRDeName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Designation----"
            };
            lstDesignation.Insert(0, defItem);
            return lstDesignation;
        }
        private List<SelectListItem> GetHRWStatus()
        {
            var lstStatus = new List<SelectListItem>();
            PaginatedList<HRWStatus> items = _hrwstatusRepo.GetItems(1, 1000);
            lstStatus = items.Select(ut => new SelectListItem()
            {
                Value = ut.HRWSId.ToString(),
                Text = ut.HRWSName
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Work Status----"
            };
            lstStatus.Insert(0, defItem);
            return lstStatus;
        }
        [AcceptVerbs("Get","Post")]
        public JsonResult IsEmpCodeExists(string empcode)
        {
            bool isExists = _hrempdetailsRepo.IsEmpCodeExists(empcode);
            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult IsEmpCodeExists(string empcode, int id)
        {
            bool isExists = _hrempdetailsRepo.IsEmpCodeExists(empcode, id);

            if (isExists)
                return Json(data: false);
            else
                return Json(data: true);
        }

        public string UploadImage(HREmpDetails empdetails)
        {
            string filename = null;
            if (empdetails.HREDPPhoto != null)
            {
                string UploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                filename = Guid.NewGuid().ToString() + "_" + empdetails.HREDPPhoto.FileName;
                string filePath = Path.Combine(UploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    empdetails.HREDPPhoto.CopyTo(fileStream);

                }
            }
            return filename;
        }
    }
}
