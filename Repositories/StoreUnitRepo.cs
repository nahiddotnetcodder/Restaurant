using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreUnitRepo : IStoreUnit
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreUnitRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(StoreUnit model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var allData = await _context.StoreUnit.Where(x => x.SUName == model.SUName || x.SUId == model.SUId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.SUId = 0;
            model.CUser = currentUser.Id;
            _context.StoreUnit.Add(model);
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

        public async Task<StoreUnit> GetById(int id)
        {
            var role = await _context.StoreUnit.Where(x => x.SUId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(StoreUnit model)
        {
            var status = new ResponseStatus();
            var allData = await _context.StoreUnit.Where(x => x.SUName == model.SUName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            var data = await _context.StoreUnit.Where(x => x.SUId == model.SUId).FirstOrDefaultAsync();

            data.SUId = model.SUId;
            data.SUName = model.SUName;
            _context.StoreUnit.Update(data);

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
            var data = await _context.StoreUnit.Where(x => x.SUId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.StoreUnit.Remove(data);
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
        public async Task<List<StoreUnit>> GetAll()
        {
            var roles = await _context.StoreUnit.ToListAsync();
            return roles;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<StoreUnit> GetItems( int pageIndex = 1, int pageSize = 10)
        {
            List<StoreUnit> items;
            items = _context.StoreUnit.ToList();
            PaginatedList<StoreUnit> retItems = new PaginatedList<StoreUnit>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.StoreUnit.Where(n => n.SUName.ToLower() == name.ToLower() && n.SUId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
