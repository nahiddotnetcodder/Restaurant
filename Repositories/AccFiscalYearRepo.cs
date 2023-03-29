using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class AccFiscalYearRepo : IAccFiscalYear
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccFiscalYearRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetRefPrefixNo()
        {
            var data = await _context.AccJournal.OrderByDescending(x => x.AJId).LastOrDefaultAsync();
            if(data == null)
                return 1.ToString().PadLeft(4, '0'); ;
            int serial = Convert.ToInt32(data.AJRefPrefix);
            return serial.ToString().PadLeft(4, '0');
        }

        public async Task<ResponseStatus> Create(AccFiscalYear model)
        {
            var status = new ResponseStatus();

            var daysDiff = (model.AFYEndDate - model.AFYBeginDate).TotalDays;
            if (daysDiff < 364)
            {
                status.StatusCode = 0;
                status.Message = "Fiscal Year Duration Must be 1 Year";
                return status;
            }
            var currentUser = GetCurrentUser();

            model.AFYId = 0;
            model.CUser = currentUser.Id;
            _context.AccFiscalYear.Add(model);
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

        public async Task<AccFiscalYear> GetById(int id)
        {
            var role = await _context.AccFiscalYear.Where(x=> x.AFYId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(AccFiscalYear model)
        {
            var status = new ResponseStatus();
            var data = await _context.AccFiscalYear.Where(x => x.AFYId == model.AFYId).FirstOrDefaultAsync();

            data.AFYClosed = model.AFYClosed;
            _context.AccFiscalYear.Update(data);

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
            var data = await _context.AccFiscalYear.Where(x => x.AFYId == id).FirstOrDefaultAsync();

           if(data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Not Found";
                return status;
            }
            _context.AccFiscalYear.Remove(data);
            var result = await _context.SaveChangesAsync();
            if (result < 1)
            {
                status.StatusCode = 0;
                status.Message = "Entity Deletion Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "Entity Deletion successfully";
            return status;
        }
        public async Task<List<AccFiscalYear>> GetAll()
        {
            var roles = await _context.AccFiscalYear.ToListAsync();
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
