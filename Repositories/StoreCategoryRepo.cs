
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RMS.Interfaces;
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreCategoryRepo : IStoreCategory
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreCategoryRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(StoreCategory model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var allData = await _context.StoreCategory.Where(x => x.SCName == model.SCName || x.SCId == model.SCId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Category Name already exists";
                return status;
            }

            model.SCId = 0;
            model.CUser = currentUser.Id;
            _context.StoreCategory.Add(model);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                status.StatusCode = 0;
                status.Message = "Entity Creation Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "Entity Created successfully";
            return status;
        }

        public async Task<StoreCategory> GetById(int id)
        {
            var role = await _context.StoreCategory.Where(x => x.SCId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(StoreCategory model)
        {
            var status = new ResponseStatus();
            var allData = await _context.StoreCategory.Where(x => x.SCName == model.SCName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Category Name already exists";
                return status;
            }
            var data = await _context.StoreCategory.Where(x => x.SCId == model.SCId).FirstOrDefaultAsync();

            data.SCId = model.SCId;
            data.SCName = model.SCName;
            _context.StoreCategory.Update(data);

            var result = await _context.SaveChangesAsync();
            if (result < 1)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "Entity Updated successfully";
            return status;
        }
        public async Task<ResponseStatus> Delete(int id)
        {
            var status = new ResponseStatus();
            var data = await _context.StoreCategory.Where(x => x.SCId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.StoreCategory.Remove(data);
            var result = await _context.SaveChangesAsync();
            if (result < 1)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "Entity Updated successfully";
            return status;
        }
        public async Task<List<StoreCategory>> GetAll()
        {
            var roles = await _context.StoreCategory.ToListAsync();
            return roles;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<StoreCategory> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<StoreCategory> items;
            items = _context.StoreCategory.ToList();
            PaginatedList<StoreCategory> retItems = new PaginatedList<StoreCategory>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.StoreCategory.Where(n => n.SCName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.StoreCategory.Where(n => n.SCName.ToLower() == name.ToLower() && n.SCId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
