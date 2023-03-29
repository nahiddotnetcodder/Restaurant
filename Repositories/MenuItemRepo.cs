using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class MenuItemRepo : IMenuItem
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MenuItemRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(MenuItem model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            var allData = await _context.MenuItem.Where(x => x.Name == model.Name).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.Id = 0;
            model.CUser = currentUser.FullName;
            _context.MenuItem.Add(model);
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

        public async Task<MenuItem> GetById(int id)
        {
            var data = await _context.MenuItem.Include(ch => ch.Menu).Where(x=> x.Id == id).ToListAsync();
            var result = data.Select(x => new MenuItem
            {
                Id = x.Id,
                MenuId = x.MenuId,
                Name = x.Name,
                StatusId = x.StatusId,
                MenuName = x.Menu.Name
            }).FirstOrDefault();
            return result;
        }

        public async Task<ResponseStatus> Update(MenuItem model)
        {
            var status = new ResponseStatus();
            var data = await _context.MenuItem.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            data.MenuId = model.MenuId;
            data.StatusId = model.StatusId;
            data.Name = model.Name;
            _context.MenuItem.Update(data);

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

            var data = await _context.MenuItem.Where(x => x.Id == id).FirstOrDefaultAsync();

           if(data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.MenuItem.Remove(data);
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
        public async Task<List<MenuItem>> GetAll()
        {
            var data = await _context.MenuItem.Include(x=> x.Menu).ToListAsync();
            var result = data.Select(x => new MenuItem
            {
                Id = x.Id,
                MenuId = x.MenuId,
                Name = x.Name,
                StatusId = x.StatusId,
                MenuName = x.MenuName
            }).ToList();
            return result;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }
    }
}
