using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class AccChartMasterRepo : IAccChartMaster
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccChartMasterRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(AccChartMaster model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            model.CUser = currentUser.Id;
            model.IsActive = model.ACMAI == ACMAI.Active;
            _context.AccChartMaster.Add(model);
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

        public async Task<AccChartMaster> GetById(int id)
        {
            var role = await _context.AccChartMaster.Include(ch => ch.AccChartTypes).Where(x=> x.ACMId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(AccChartMaster model)
        {
            var status = new ResponseStatus();
            var data = await _context.AccChartMaster.Where(x => x.ACMId == model.ACMId).FirstOrDefaultAsync();

            data.ACMAccName = model.ACMAccName;
            data.ACTId = model.ACTId;
            data.ACMAI = model.ACMAI;
            data.IsActive = model.ACMAI == ACMAI.Active;
            _context.AccChartMaster.Update(data);

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
            var data = await _context.AccChartMaster.Where(x => x.ACMId == id).FirstOrDefaultAsync();

           if(data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.AccChartMaster.Remove(data);
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
        public async Task<List<AccChartMaster>> GetAll()
        {
            var roles = await _context.AccChartMaster.Where(pm => pm.IsActive).Include(x=> x.AccChartTypes).ToListAsync();
            return roles;
        }
        public async Task<List<AccChartMaster>> GetAllIncludeInactive()
        {
            var roles = await _context.AccChartMaster.Include(x => x.AccChartTypes).ToListAsync();
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
