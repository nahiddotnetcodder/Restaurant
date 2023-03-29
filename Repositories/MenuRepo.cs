using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class MenuRepo : IMenu
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MenuRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(Menu model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            var allData = await _context.Menu.Where(x => x.Name == model.Name).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.Id = 0;
            model.CUser = currentUser.FullName;
            _context.Menu.Add(model);
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

        public async Task<Menu> GetById(int id)
        {
            var role = await _context.Menu.Where(x => x.Id == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(Menu model)
        {
            var status = new ResponseStatus();
            var data = await _context.Menu.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            data.Name = model.Name;
            data.StatusId = model.StatusId;
            _context.Menu.Update(data);

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


            var menuItem = await _context.MenuItem.Where(x => x.MenuId == id).FirstOrDefaultAsync();
            if (menuItem != null)
            {
                status.StatusCode = 0;
                status.Message = "Can't Delete Has a DB Relation with Acc Master";
                return status;
            }

            var data = await _context.Menu.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.Menu.Remove(data);
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
        public async Task<List<Menu>> GetAll()
        {
            var roles = await _context.Menu.ToListAsync();
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
