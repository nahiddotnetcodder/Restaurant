
using Newtonsoft.Json;
using RMS.Interfaces;
using RMS.Models;

namespace RMS.Repositories
{
    public class HREmpSalaryRepo : IHREmpSalary
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HREmpSalaryRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public HREmpSalary Create(HREmpSalary hrsalary)
        {

            var salaryPolicy = _context.HRSalaryPolicy.ToList();
            var empName = _context.HREmpDetails.Where(x=>x.HREDId == hrsalary.HREDId).FirstOrDefault();
            if(empName != null)
            {
                if (salaryPolicy != null)
                {
                    int sum = 0;
                    foreach (var item in salaryPolicy)
                    {
                        var basic = empName.HREDBasic;
                        sum += (int)(basic * item.HRSPPercent / 100);
                    }
                    hrsalary.HRSBasic = empName.HREDBasic;
                    hrsalary.HRSGTotal = empName.HREDBasic + sum;
                }
            }
            else
            {
                var allEmp = _context.HREmpDetails.ToList();
                foreach (var item in allEmp)
                {
                    int sum = 0;
                    foreach (var items in salaryPolicy)
                    {
                        var basic = item.HREDBasic;
                        sum += (int)(basic * items.HRSPPercent / 100);
                    }
                    hrsalary.HREDId = item.HREDId;
                    hrsalary.HRSBasic = item.HREDBasic;
                    hrsalary.HRSGTotal = item.HREDBasic + sum;
                }
            }


            var currentUser = GetCurrentUser();
            hrsalary.HRSId = 0;
            hrsalary.CUser = currentUser.FullName;
            _context.HRSalary.Add(hrsalary);
            _context.SaveChanges();
            return hrsalary;
        }

        public HREmpSalary Delete(HREmpSalary hrsalary)
        {
            _context.HRSalary.Attach(hrsalary);
            _context.Entry(hrsalary).State = EntityState.Deleted;
            _context.SaveChanges();
            return hrsalary;
        }

        public HREmpSalary Edit(HREmpSalary hrsalary)
        {
            _context.HRSalary.Attach(hrsalary);
            _context.Entry(hrsalary).State = EntityState.Modified;
            _context.SaveChanges();
            return hrsalary;
        }

        public HREmpSalary GetItem(int id)
        {
            HREmpSalary item = _context.HRSalary.Where(u => u.HRSId == id).FirstOrDefault();
            return item;
        }

        public PaginatedList<HREmpSalary> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<HREmpSalary> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.HRSalary.Where(n => n.HREmpDetails.HREDEName.Contains(SearchText) )
                    .Include(u=> u.HREmpDetails)
                    .ToList();
            }
            else
                items = _context.HRSalary.Include(u=>u.HREmpDetails).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<HREmpSalary> retItems = new PaginatedList<HREmpSalary>(items, pageIndex, pageSize);
            return retItems;
        }
        private List<HREmpSalary> DoSort(List<HREmpSalary> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "Name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.HREmpDetails.HREDEName).ToList();
                else
                    items = items.OrderByDescending(n => n.HREmpDetails.HREDEName).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.HRSYear).ToList();
                else
                    items = items.OrderByDescending(d => d.HRSYear).ToList();
            }
            return items;
        }
        public bool IsDNameExists(int id)
        {
            int ct = _context.HRSalary.Where(n => n.HREDId == id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsDNameExists(int name, int Id)
        {

            int ct = _context.HRSalary.Where(n => n.HREDId == name && n.HRSId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }
    }
}
