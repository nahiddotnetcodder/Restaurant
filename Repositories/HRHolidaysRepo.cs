using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HRHolidaysRepo : IHRHolidays
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRHolidaysRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HRHolidays model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            model.HRHId = 0;
            model.CUser = currentUser.Id;
            _context.HRHolidays.Add(model);
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

        public async Task<HRHolidays> GetById(int id)
        {
            var role = await _context.HRHolidays.Where(x => x.HRHId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HRHolidays model)
        {
            var status = new ResponseStatus();
            var data = await _context.HRHolidays.Where(x => x.HRHId == model.HRHId).FirstOrDefaultAsync();

            data.HRHId = model.HRHId;
            data.HolidayType = model.HolidayType;
            data.HRHStartDate = model.HRHStartDate;
            data.HRHEndDate = model.HRHEndDate;
            _context.HRHolidays.Update(data);

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
            var data = await _context.HRHolidays.Where(x => x.HRHId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HRHolidays.Remove(data);
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
        public async Task<List<HRHolidays>> GetAll()
        {
            var roles = await _context.HRHolidays.ToListAsync();
            return roles;

            //var data = await _context.HRHolidays.Include(x => x.HREmpDetails).ToListAsync();
            //var result = data.Select(x => new HREmpRoaster
            //{
            //    HRERId = x.HRERId,
            //    HREmpDetailsName = x.HREmpDetails == null ? string.Empty : x.HREmpDetails.HREDEName,
            //    HRERFDate = x.HRERFDate,
            //    HRERTDate = x.HRERTDate,
            //    ShiftType = x.ShiftType
            //}).ToList();
            //return result;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<HRHolidays> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HRHolidays> items;
            items = _context.HRHolidays.ToList();
            PaginatedList<HRHolidays> retItems = new PaginatedList<HRHolidays>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(int? id)
        {
            int ct = _context.HRHolidays.Where(n => n.HRHId == id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.HRHolidays.Where(n => n.HolidayTypeName.ToLower() == name.ToLower() && n.HRHId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
