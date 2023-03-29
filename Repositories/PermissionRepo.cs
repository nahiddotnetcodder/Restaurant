using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class PermissionRepo : IPermission
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PermissionRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(AccPermission model)
        {
            var status = new ResponseStatus();

            var permissin_key = await _context.AccPermission.Where(x => x.Key == model.Key).FirstOrDefaultAsync();
            if(permissin_key != null)
            {
                status.StatusCode = 1;
                status.Message = "Permission already used";
                return status;
            }

            var currentUser = GetCurrentUser();

            model.CUser = currentUser.Id;
            _context.AccPermission.Add(model);
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

        public async Task<AccPermission> GetById(int id)
        {
            var role = await _context.AccPermission.Where(x=> x.Id == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(AccPermission model)
        {
            var status = new ResponseStatus();
            var data = await _context.AccPermission.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            data.Key = model.Key;
            data.MenuName = model.MenuName;
            data.MenuItem = model.MenuItem;
            data.Description = model.Description;
            _context.AccPermission.Update(data);

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
            var data = await _context.AccPermission.Where(x => x.Id == id).FirstOrDefaultAsync();

           if(data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.AccPermission.Remove(data);
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
        public async Task<List<AccPermission>> GetAll()
        {
            var roles = await _context.AccPermission.ToListAsync();
            return roles;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }
    }
}
