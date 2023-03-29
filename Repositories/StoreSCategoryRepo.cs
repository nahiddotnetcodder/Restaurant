using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreSCategoryRepo : IStoreSCategory
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreSCategoryRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(StoreSCategory model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var allData = await _context.StoreSCategory.Where(x => x.SSCName == model.SSCName && x.SCId == model.SCId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Sub-Category Name already exists";
                return status;
            }

            model.SSCId = 0;
            model.CUser = currentUser.Id;
            _context.StoreSCategory.Add(model);
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

        public async Task<StoreSCategory> GetById(int id)
        {
            var role = await _context.StoreSCategory.Where(x => x.SSCId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(StoreSCategory model)
        {
            var status = new ResponseStatus();
            var currentUser = GetCurrentUser();

            var allData = await _context.StoreSCategory.Where(x => x.SSCName == model.SSCName ).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Sub-Category Name already exists";
                return status;
            }

            var data = await _context.StoreSCategory.Where(x => x.SSCId == model.SSCId).FirstOrDefaultAsync();

            data.SSCId = model.SSCId;
            data.SCId = model.SCId;
            data.SSCName = model.SSCName;
    
            _context.StoreSCategory.Update(data);

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
            var data = await _context.StoreSCategory.Where(x => x.SSCId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.StoreSCategory.Remove(data);
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
        public async Task<List<StoreSCategory>> GetAll()
        {
            //var roles = await _context.HREmpRoaster.ToListAsync();
            //return roles;

            var data = await _context.StoreSCategory.Include(x => x.StoreCat).ToListAsync();
            var result = data.Select(x => new StoreSCategory
            {
                SSCId = x.SSCId,
                StoreCatName = x.StoreCat == null ? string.Empty : x.StoreCat.SCName,
                SSCName = x.SSCName,
            }).ToList();
            return result;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<StoreSCategory> GetItems(int pageIndex = 1, int pageSize = 10)
        {
            List<StoreSCategory> items;
            items = _context.StoreSCategory.ToList();
            PaginatedList<StoreSCategory> retItems = new PaginatedList<StoreSCategory>(items, pageIndex, pageSize);
            return retItems;
        }


        public bool IsItemExists(string name, int id)
        {
            int ct = _context.StoreSCategory.Where(n => n.SSCName.ToLower() == name.ToLower() && n.SSCId != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(int? id)
        {
            int ct = _context.HREmpRoaster.Where(n => n.HREDId == id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
