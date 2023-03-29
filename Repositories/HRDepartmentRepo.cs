using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HRDepartmentRepo : IHRDepartment
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRDepartmentRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HRDepartment model)
        {
            var status = new ResponseStatus();
            var allData = await _context.HRDepartment.Where(x => x.HRDName == model.HRDName || x.HRDId == model.HRDId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Department Name already exists";
                return status;
            }

            model.HRDId = 0;
            model.HRDDes = model.HRDDes == null ? string.Empty : model.HRDDes;
            _context.HRDepartment.Add(model);
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

        public async Task<HRDepartment> GetById(int id)
        {
            var role = await _context.HRDepartment.Where(x => x.HRDId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HRDepartment model)
        {
            var status = new ResponseStatus();
            var allData = await _context.HRDepartment.Where(x => x.HRDName == model.HRDName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Department Name already exists";
                return status;
            }
            var data = await _context.HRDepartment.Where(x => x.HRDId == model.HRDId).FirstOrDefaultAsync();

            data.HRDId = model.HRDId;
            data.HRDName = model.HRDName;
            data.HRDDes = model.HRDDes == null ? string.Empty : model.HRDDes;
            _context.HRDepartment.Update(data);

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
            var data = await _context.HRDepartment.Where(x => x.HRDId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HRDepartment.Remove(data);
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
        public async Task<List<HRDepartment>> GetAll()
        {
            var roles = await _context.HRDepartment.ToListAsync();
            return roles;
        }

        public PaginatedList<HRDepartment> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HRDepartment> items;
            items = _context.HRDepartment.ToList();
            PaginatedList<HRDepartment> retItems = new PaginatedList<HRDepartment>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.HRDepartment.Where(n => n.HRDName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.HRDepartment.Where(n => n.HRDName.ToLower() == name.ToLower() && n.HRDId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
