using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HRWeekendRepo : IHRWeekend
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRWeekendRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HRWeekend model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var allData = await _context.HRWeekend.Where(x => x.Weekday == model.Weekday || x.HRWId == model.HRWId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Weekend Name already exists";
                return status;
            }

            model.HRWId = 0;
            model.CUser = currentUser.Id;
            _context.HRWeekend.Add(model);
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

        public async Task<HRWeekend> GetById(int id)
        {
            var role = await _context.HRWeekend.Where(x => x.HRWId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HRWeekend model)
        {
            var status = new ResponseStatus();
            var data = await _context.HRWeekend.Where(x => x.HRWId == model.HRWId).FirstOrDefaultAsync();

            data.HRWId = model.HRWId;
            data.Weekday = model.Weekday;
            _context.HRWeekend.Update(data);

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
            var data = await _context.HRWeekend.Where(x => x.HRWId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HRWeekend.Remove(data);
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
        public async Task<List<HRWeekend>> GetAll()
        {
            var roles = await _context.HRWeekend.ToListAsync();
            return roles;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<HRWeekend> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HRWeekend> items;
            items = _context.HRWeekend.ToList();
            PaginatedList<HRWeekend> retItems = new PaginatedList<HRWeekend>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(int id)
        {
            int ct = _context.HRWeekend.Where(n => n.HRWId == id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(int name, int id)
        {
            int ct = _context.HRWeekend.Where(n => n.Weekday.ToString() == name.ToString() && n.HRWId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
