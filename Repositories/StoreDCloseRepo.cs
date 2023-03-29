


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RMS.Models;

namespace RMS.Repositories
{
    public class StoreDCloseRepo : IStoreDClose
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreDCloseRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public StoreDClose Create(StoreDClose resdclose)
        {

            var currentUser = GetCurrentUser();

            resdclose.SDCId = 0;
            resdclose.CUser = currentUser.Id;

            _context.StoreDClose.Add(resdclose);
            _context.SaveChanges();
            return resdclose;
        }
        public StoreDClose Edit(StoreDClose resdclose)
        {         
            _context.StoreDClose.Attach(resdclose);
            _context.Entry(resdclose).State = EntityState.Modified;
            _context.SaveChanges();
            return resdclose;
        }
        private List<StoreDClose> DoSort(List<StoreDClose> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "CUser")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.CUser).ToList();
                else
                    items = items.OrderByDescending(n => n.CUser).ToList();
            }
            return items;
        }
        public PaginatedList<StoreDClose> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<StoreDClose> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.StoreDClose.Where(n => n.CUser.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.StoreDClose.ToList();
                items = DoSort(items, SortProperty, sortOrder);
                PaginatedList<StoreDClose> retItems = new PaginatedList<StoreDClose>(items, pageIndex, pageSize);
                return retItems;
        }
        public StoreDClose GetItem(int rdcid)
        {
            StoreDClose item = _context.StoreDClose.Where(u => u.SDCId == rdcid).FirstOrDefault();
            return item;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public DateTime getDate()
        {
            try
            {
                var lastDate = _context.StoreDClose.Max(n => n.SDCDate);
                return lastDate.AddDays(1);
            }
            catch(Exception)
            {
                 return DateTime.Now.Date;
            }      
        }
    }
}
