
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.Repositories
{
    public class StoreIGenRepo : IStoreIGen
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreIGenRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public StoreIGen Create(StoreIGen storeIGen)
        {
            var currentUser = GetCurrentUser();
            storeIGen.SIGId = 0;
            storeIGen.CUser = currentUser.FullName;

            _context.StoreIGen.Add(storeIGen);
            _context.SaveChanges();
            return storeIGen;
        }

        public StoreIGen Delete(StoreIGen storeIGen)
        {
            _context.StoreIGen.Attach(storeIGen);
            _context.Entry(storeIGen).State = EntityState.Deleted;
            _context.SaveChanges();
            return storeIGen;
        }

        public StoreIGen Edit(StoreIGen storeIGen)
        {
            var currentUser = GetCurrentUser();
            storeIGen.CUser = currentUser.Id;
            _context.StoreIGen.Attach(storeIGen);
            _context.Entry(storeIGen).State = EntityState.Modified;
            _context.SaveChanges();
            return storeIGen;

        }

        public StoreIGen GetItem(int rdcid)
        {
            StoreIGen item = _context.StoreIGen.Where(u => u.SIGId == rdcid).FirstOrDefault();
            return item;
        }

        public PaginatedList<StoreIGen> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<StoreIGen> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.StoreIGen.Where(n => n.SIGItemCode.Contains(SearchText) || n.StoreSCategory.SSCName.Contains(SearchText) )
                    .Include(u=> u.StoreSCategory)
                    .ToList();
            }
            else
            items = _context.StoreIGen.Include(u=> u.StoreCategory).ToList();
            items = _context.StoreIGen.Include(u=> u.StoreSCategory).ToList();
            items = _context.StoreIGen.Include(u=> u.StoreUnits).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<StoreIGen> retItems = new PaginatedList<StoreIGen>(items, pageIndex, pageSize);
            return retItems;
        }

        private List<StoreIGen> DoSort(List<StoreIGen> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "Name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.SIGItemCode).ToList();
                else
                    items = items.OrderByDescending(n => n.SIGItemCode).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.SIGItemName).ToList();
                else
                    items = items.OrderByDescending(d => d.SIGItemName).ToList();
            }
            return items;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.StoreIGen.Where(n => n.SIGItemName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string code, int id)
        {
            int ct = _context.StoreIGen.Where(n => n.SIGItemCode.ToLower() == code.ToLower() && n.SIGId != id).Count();
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
        public async Task<List<StoreIGen>> GetAll()
        {
            var roles = await _context.StoreIGen.Include(x=>x.StoreUnits).ToListAsync();
            return roles;
        }

       
    }
}
