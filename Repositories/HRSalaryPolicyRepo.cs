using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HRSalaryPolicyRepo : IHRSalaryPolicy
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRSalaryPolicyRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HRSalaryPolicy model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var allData = await _context.HRSalaryPolicy.Where(x => x.HRSPName == model.HRSPName || x.HRSPId == model.HRSPId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.HRSPId = 0;
            model.CUser = currentUser.Id;
            _context.HRSalaryPolicy.Add(model);
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

        public async Task<HRSalaryPolicy> GetById(int id)
        {
            var role = await _context.HRSalaryPolicy.Where(x => x.HRSPId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HRSalaryPolicy model)
        {
            var status = new ResponseStatus();
            var data = await _context.HRSalaryPolicy.Where(x => x.HRSPId == model.HRSPId).FirstOrDefaultAsync();

            data.HRSPId = model.HRSPId;
            data.HRSPName = model.HRSPName;
            data.ADDUC = model.ADDUC;
            data.PerNPer = model.PerNPer;
            data.HRSPPercent = model.HRSPPercent;
            _context.HRSalaryPolicy.Update(data);

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
            var data = await _context.HRSalaryPolicy.Where(x => x.HRSPId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HRSalaryPolicy.Remove(data);
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
        public async Task<List<HRSalaryPolicy>> GetAll()
        {
            var roles = await _context.HRSalaryPolicy.ToListAsync();
            return roles;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<HRSalaryPolicy> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HRSalaryPolicy> items;
            items = _context.HRSalaryPolicy.ToList();
            PaginatedList<HRSalaryPolicy> retItems = new PaginatedList<HRSalaryPolicy>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.HRSalaryPolicy.Where(n => n.HRSPName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.HRSalaryPolicy.Where(n => n.HRSPName.ToLower() == name.ToLower() && n.HRSPId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
