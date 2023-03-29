
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.Repositories
{
    public class HRLeaveDetailsRepo : IHRLeaveDetails
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRLeaveDetailsRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public HRLeaveDetail Create(HRLeaveDetail hrleavedetails)
        {
            var currentUser = GetCurrentUser();

            hrleavedetails.HRLDId = 0;
            hrleavedetails.CUser = currentUser.FullName;
            _context.HRLeaveDetail.Add(hrleavedetails);
            _context.SaveChanges();
            return hrleavedetails;
        }

        public HRLeaveDetail Delete(HRLeaveDetail hrleavedetails)
        {
            _context.HRLeaveDetail.Attach(hrleavedetails);
            _context.Entry(hrleavedetails).State = EntityState.Deleted;
            _context.SaveChanges();
            return hrleavedetails;
        }

        public HRLeaveDetail Edit(HRLeaveDetail hrleavedetails)
        {
            _context.HRLeaveDetail.Attach(hrleavedetails);
            _context.Entry(hrleavedetails).State = EntityState.Modified;
            _context.SaveChanges();
            return hrleavedetails;
        }

        public HRLeaveDetail GetItem(int id)
        {
            HRLeaveDetail item = _context.HRLeaveDetail.Where(u => u.HRLDId == id).FirstOrDefault();
            return item;
        }

        public PaginatedList<HRLeaveDetail> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<HRLeaveDetail> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.HRLeaveDetail.Where(n => n.HREmpDetails.HREDEName.Contains(SearchText) || n.HRLeavePolicy.HRLPName.Contains(SearchText))
                    .Include(u=> u.HREmpDetails)
                    .ToList();
            }
            else
            items = _context.HRLeaveDetail.Include(u=> u.HREmpDetails).ToList();
            items = _context.HRLeaveDetail.Include(u=> u.HRLeavePolicy).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<HRLeaveDetail> retItems = new PaginatedList<HRLeaveDetail>(items, pageIndex, pageSize);
            return retItems;
        }
        private List<HRLeaveDetail> DoSort(List<HRLeaveDetail> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "Name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.HREDEName).ToList();
                else
                    items = items.OrderByDescending(n => n.HREDEName).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.HRLeavePolicy.HRLPName).ToList();
                else
                    items = items.OrderByDescending(d => d.HRLeavePolicy.HRLPName).ToList();
            }
            return items;
        }
        public bool IsNameExists(int Did)
        {
            int ct = _context.HRLeaveDetail.Where(n => n.HREDId == Did).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        //public bool IsNameExists(int name, int id)
        //{

        //    int ct = _context.HRLeaveDetail.Where(n => n.HREDId == name && n.HRLDId != id).Count();
        //    if (ct > 0)
        //        return true;
        //    else
        //        return false;
        //}
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public bool IsNameExists(DateTime indate, DateTime outdate)
        {
            int ct = _context.HRLeaveDetail.Where(n => n.HRLDLeaveSDate == indate || n.HRLDLeaveEDate == outdate).Count();
            if (ct > 0)
            {
                if (indate.Date == outdate.Date)
                {
                    return true;
                }
                else
                {
                    for (var i = indate.Date; i.Date <= outdate.Date; i = i.AddDays(1))
                    {
                        return true;
                    }
                }

            }
            return false;

        }
    }
}

