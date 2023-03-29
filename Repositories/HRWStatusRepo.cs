using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HRWStatusRepo : IHRWStatus
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRWStatusRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HRWStatus model)
        {
            var status = new ResponseStatus();

            //var currentUser = GetCurrentUser();
            model.HRWSId = 0;
            model.HRWSDes = model.HRWSDes == null ? string.Empty : model.HRWSDes;
            //model.CUser = currentUser.IdHRWSDes
            var allData = await _context.HRWStatus.Where(x => x.HRWSName == model.HRWSName || x.HRWSId == model.HRWSId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Status Name already exists";
                return status;
            }
            _context.HRWStatus.Add(model);
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

        public async Task<HRWStatus> GetById(int id)
        {
            var role = await _context.HRWStatus.Where(x => x.HRWSId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HRWStatus model)
        {
            var status = new ResponseStatus();
            var allData = await _context.HRWStatus.Where(x => x.HRWSName == model.HRWSName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Status Name already exists";
                return status;
            }
            var data = await _context.HRWStatus.Where(x => x.HRWSId == model.HRWSId).FirstOrDefaultAsync();

            data.HRWSId = model.HRWSId;
            data.HRWSName = model.HRWSName;
            data.HRWSDes = model.HRWSDes == null ? string.Empty : model.HRWSDes;
            _context.HRWStatus.Update(data);

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
            var data = await _context.HRWStatus.Where(x => x.HRWSId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HRWStatus.Remove(data);
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
        public async Task<List<HRWStatus>> GetAll()
        {
            var roles = await _context.HRWStatus.ToListAsync();
            return roles;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<HRWStatus> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HRWStatus> items;
            items = _context.HRWStatus.ToList();
            PaginatedList<HRWStatus> retItems = new PaginatedList<HRWStatus>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.HRWStatus.Where(n => n.HRWSName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.HRWStatus.Where(n => n.HRWSName.ToLower() == name.ToLower() && n.HRWSId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
