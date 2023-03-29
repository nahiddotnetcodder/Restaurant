
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.Repositories
{
    public class ResMenuRepo : IResMenu
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResMenuRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public ResMenu Create(ResMenu model)
        {
            var currentUser = GetCurrentUser();
            model.RMId = 0;
            model.CUser = currentUser.FullName;

            _context.ResMenu.Add(model);
            _context.SaveChanges();
            return model;
        }

        public ResMenu Delete(ResMenu model)
        {
            _context.ResMenu.Attach(model);
            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
            return model;
        }

        public ResMenu Edit(ResMenu model)
        {
            var currentUser = GetCurrentUser();
            model.CUser = currentUser.Id;

            _context.ResMenu.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            return model;
        }

        public ResMenu GetItem(int id)
        {
            ResMenu item = _context.ResMenu.Where(u => u.RMId == id).FirstOrDefault();
            return item;
        }

        public PaginatedList<ResMenu> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<ResMenu> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.ResMenu.Where(n => n.RMItemCode.Contains(SearchText) || n.RMItemName.Contains(SearchText) )
                    .ToList();
            }
            else
            items = _context.ResMenu.Include(u=> u.ResKInfo).ToList();
            items = _context.ResMenu.Include(u=> u.ResFtype).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<ResMenu> retItems = new PaginatedList<ResMenu>(items, pageIndex, pageSize);
            return retItems;
        }

        private List<ResMenu> DoSort(List<ResMenu> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "Name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.RMItemCode).ToList();
                else
                    items = items.OrderByDescending(n => n.RMItemCode).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.RMItemName).ToList();
                else
                    items = items.OrderByDescending(d => d.RMItemName).ToList();
            }
            return items;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.ResMenu.Where(n => n.RMItemName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string code, int id)
        {
            int ct = _context.ResMenu.Where(n => n.RMItemCode.ToLower() == code.ToLower() && n.RMId != id).Count();
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
        public async Task<List<ResMenu>> GetAll()
        {
            var roles = await _context.ResMenu.ToListAsync();
            return roles;
        }
    }
}
