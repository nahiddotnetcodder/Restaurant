using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class ResFoodTypeRepo : IResFoodType
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResFoodTypeRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(ResFoodType model)
        {
            var status = new ResponseStatus();
            var allData = await _context.ResFoodType.Where(x => x.RFTName == model.RFTName || x.RFTId == model.RFTId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.RFTId = 0;
            _context.ResFoodType.Add(model);
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

        public async Task<ResFoodType> GetById(int id)
        {
            var role = await _context.ResFoodType.Where(x => x.RFTId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(ResFoodType model)
        {
            var status = new ResponseStatus();

            var allData = await _context.ResFoodType.Where(x => x.RFTName == model.RFTName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }
            var data = await _context.ResFoodType.Where(x => x.RFTId == model.RFTId).FirstOrDefaultAsync();

            data.RFTId = model.RFTId;
            data.RFTName = model.RFTName;
            data.RFTDescription = model.RFTDescription;
            _context.ResFoodType.Update(data);

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
            var data = await _context.ResFoodType.Where(x => x.RFTId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.ResFoodType.Remove(data);
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
        public async Task<List<ResFoodType>> GetAll()
        {
            var roles = await _context.ResFoodType.ToListAsync();
            return roles;
        }

        public PaginatedList<ResFoodType> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<ResFoodType> items;
            items = _context.ResFoodType.ToList();
            PaginatedList<ResFoodType> retItems = new PaginatedList<ResFoodType>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.ResFoodType.Where(n => n.RFTName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.ResFoodType.Where(n => n.RFTName.ToLower() == name.ToLower() && n.RFTId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
