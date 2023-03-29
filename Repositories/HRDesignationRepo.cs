using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class HRDesignationRepo : IHRDesignation
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRDesignationRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(HRDesignation model)
        {
            var status = new ResponseStatus();
            var allData = await _context.HRDesignation.Where(x => x.HRDeName == model.HRDeName || x.HRDeId == model.HRDeId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Designation Name already exists";
                return status;
            }

            model.HRDeId = 0;
            model.HRDeDes = model.HRDeDes == null ? string.Empty : model.HRDeDes;
            _context.HRDesignation.Add(model);
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

        public async Task<HRDesignation> GetById(int id)
        {
            var role = await _context.HRDesignation.Where(x => x.HRDeId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(HRDesignation model)
        {
            var status = new ResponseStatus();

            var allData = await _context.HRDesignation.Where(x => x.HRDeName == model.HRDeName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Designation Name already exists";
                return status;
            }
            var data = await _context.HRDesignation.Where(x => x.HRDeId == model.HRDeId).FirstOrDefaultAsync();

            data.HRDeId = model.HRDeId;
            data.HRDeName = model.HRDeName;
            data.HRDeDes = model.HRDeDes == null ? string.Empty : model.HRDeDes;
            _context.HRDesignation.Update(data);

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
            var data = await _context.HRDesignation.Where(x => x.HRDeId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.HRDesignation.Remove(data);
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
        public async Task<List<HRDesignation>> GetAll()
        {
            var roles = await _context.HRDesignation.ToListAsync();
            return roles;
        }

        public PaginatedList<HRDesignation> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<HRDesignation> items;
            items = _context.HRDesignation.ToList();
            PaginatedList<HRDesignation> retItems = new PaginatedList<HRDesignation>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.HRDesignation.Where(n => n.HRDeName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.HRDesignation.Where(n => n.HRDeName.ToLower() == name.ToLower() && n.HRDeId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
