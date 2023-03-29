
using Newtonsoft.Json;
using RMS.Interfaces;
using RMS.Models;

namespace RMS.Repositories
{
    public class HREmpAttRepo : IHREmpAtt
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HREmpAttRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public HREmpAtt Create(HREmpAtt hrempatt)
        {
            var currentUser = GetCurrentUser();

            hrempatt.HREAId = 0;
            hrempatt.CUser = currentUser.Id;
            _context.HREmpAtt.Add(hrempatt);
            _context.SaveChanges();
            return hrempatt;
        }

        public HREmpAtt Delete(HREmpAtt hrempatt)
        {
            _context.HREmpAtt.Attach(hrempatt);
            _context.Entry(hrempatt).State = EntityState.Deleted;
            _context.SaveChanges();
            return hrempatt;
        }

        public HREmpAtt Edit(HREmpAtt hrempatt)
        {
            var currentUser = GetCurrentUser();
            hrempatt.CUser = currentUser.Id;

            _context.HREmpAtt.Attach(hrempatt);
            _context.Entry(hrempatt).State = EntityState.Modified;
            _context.SaveChanges();
            return hrempatt;
        }

        public HREmpAtt GetItem(int id)
        {
            HREmpAtt item = _context.HREmpAtt.Where(u => u.HREAId == id).FirstOrDefault();
            return item;
        }

        public PaginatedList<HREmpAtt> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<HREmpAtt> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.HREmpAtt.Where(n => n.HREmpDetails.HREDEName.Contains(SearchText))
                    .Include(u=> u.HREmpDetails)
                    .ToList();
            }
            else
            items = _context.HREmpAtt.Include(u=> u.HREmpDetails).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<HREmpAtt> retItems = new PaginatedList<HREmpAtt>(items, pageIndex, pageSize);
            return retItems;
        }
        private List<HREmpAtt> DoSort(List<HREmpAtt> items, string SortProperty, SortOrder sortOrder)
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
                    items = items.OrderBy(d => d.HREATMinute).ToList();
                else
                    items = items.OrderByDescending(d => d.HREATMinute).ToList();
            }
            return items;
        }

        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public bool IsENameExists(string name)
        {
            int ct = _context.HREmpAtt.Where(n => n.HREAId.ToString().ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsENameExists(int id, DateTime date)
        {
            int ct = _context.HREmpAtt.Where(n => n.HREDId == id && n.HREADate == date).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
