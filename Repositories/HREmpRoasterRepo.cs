using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HREmpRoasterRepo : IHREmpRoaster
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HREmpRoasterRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HREmpRoaster model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            var allData = await _context.HREmpRoaster.Where(x => x.HREDId == model.HREDId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.HRERId = 0;
            model.CUser = currentUser.Id;

            _context.HREmpRoaster.Add(model);
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

        public async Task<HREmpRoaster> GetById(int id)
        {
            var role = await _context.HREmpRoaster.Where(x => x.HRERId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HREmpRoaster model)
        {
            var status = new ResponseStatus();
            var data = await _context.HREmpRoaster.Where(x => x.HRERId == model.HRERId).FirstOrDefaultAsync();

            data.HRERId = model.HRERId;
            data.HREDId = model.HREDId;
            data.HRERFDate = model.HRERFDate;
            data.HRERTDate = model.HRERTDate;
            data.ShiftType = model.ShiftType;
            _context.HREmpRoaster.Update(data);

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
            var data = await _context.HREmpRoaster.Where(x => x.HRERId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HREmpRoaster.Remove(data);
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
        public async Task<List<HREmpRoaster>> GetAll()
        {
            //var roles = await _context.HREmpRoaster.ToListAsync();
            //return roles;

            var data = await _context.HREmpRoaster.Include(x => x.HREmpDetails).ToListAsync();
            var result = data.Select(x => new HREmpRoaster
            {
                HRERId = x.HRERId,
                HREmpDetailsName = x.HREmpDetails == null ? string.Empty : x.HREmpDetails.HREDEName,
                HRERFDate = x.HRERFDate,
                HRERTDate = x.HRERTDate,
                ShiftType = x.ShiftType
            }).ToList();
            return result;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<HREmpRoaster> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HREmpRoaster> items;
            items = _context.HREmpRoaster.ToList();
            PaginatedList<HREmpRoaster> retItems = new PaginatedList<HREmpRoaster>(items, pageIndex, pageSize);
            return retItems;
        }

       
        public bool IsItemExists(string name, int id)
        {
            int ct = _context.HREmpRoaster.Where(n => n.HREmpDetails.HREDEName.ToLower() == name.ToLower() && n.HRERId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(int ? id)
        {
            int ct = _context.HREmpRoaster.Where(n => n.HREDId == id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
