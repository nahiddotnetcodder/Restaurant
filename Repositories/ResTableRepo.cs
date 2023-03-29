using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class ResTableRepo : IResTable
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResTableRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(ResTable model)
        {
            var status = new ResponseStatus();

            var allData = await _context.ResTable.Where(x => x.RTNumber == model.RTNumber || x.RTId == model.RTId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.RTId = 0;
            _context.ResTable.Add(model);
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

        public async Task<ResTable> GetById(int id)
        {
            var role = await _context.ResTable.Where(x => x.RTId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(ResTable model)
        {
            var status = new ResponseStatus();

            var allData = await _context.ResTable.Where(x => x.RTNumber == model.RTNumber ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }
            var data = await _context.ResTable.Where(x => x.RTId == model.RTId).FirstOrDefaultAsync();

            data.RTId = model.RTId;
            data.RTNumber = model.RTNumber;
            data.RTDescription = model.RTDescription;
            _context.ResTable.Update(data);

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
            var data = await _context.ResTable.Where(x => x.RTId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.ResTable.Remove(data);
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
        public async Task<List<ResTable>> GetAll()
        {
            var roles = await _context.ResTable.ToListAsync();
            return roles;
        }

        public PaginatedList<ResTable> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<ResTable> items;
            items = _context.ResTable.ToList();
            PaginatedList<ResTable> retItems = new PaginatedList<ResTable>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool IsItemExists(string number)
        {
            int ct = _context.ResTable.Where(n => n.RTNumber.ToLower() == number.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string number, int id)
        {
            int ct = _context.ResTable.Where(n => n.RTNumber.ToLower() == number.ToLower() && n.RTId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
