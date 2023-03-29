using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class ResKitchenInfoRepo : IResKitchenInfo
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResKitchenInfoRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(ResKitchenInfo model)
        {
            var status = new ResponseStatus();
            var allData = await _context.ResKitchenInfo.Where(x => x.RKitchenName == model.RKitchenName || x.RKId == model.RKId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.RKId = 0;
            _context.ResKitchenInfo.Add(model);
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

        public async Task<ResKitchenInfo> GetById(int id)
        {
            var role = await _context.ResKitchenInfo.Where(x => x.RKId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(ResKitchenInfo model)
        {
            var status = new ResponseStatus();
            var allData = await _context.ResKitchenInfo.Where(x => x.RKitchenName == model.RKitchenName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            var data = await _context.ResKitchenInfo.Where(x => x.RKId == model.RKId).FirstOrDefaultAsync();

            data.RKId = model.RKId;
            data.RKitchenName  = model.RKitchenName;
            data.RKDescription = model.RKDescription;
            _context.ResKitchenInfo.Update(data);

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
            var data = await _context.ResKitchenInfo.Where(x => x.RKId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.ResKitchenInfo.Remove(data);
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
        public async Task<List<ResKitchenInfo>> GetAll()
        {
            var roles = await _context.ResKitchenInfo.ToListAsync();
            return roles;
        }

        public PaginatedList<ResKitchenInfo> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<ResKitchenInfo> items;
            items = _context.ResKitchenInfo.ToList();
            PaginatedList<ResKitchenInfo> retItems = new PaginatedList<ResKitchenInfo>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.ResKitchenInfo.Where(n => n.RKitchenName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.ResKitchenInfo.Where(n => n.RKitchenName.ToLower() == name.ToLower() && n.RKId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
