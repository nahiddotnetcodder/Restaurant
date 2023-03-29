using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class AccChartRepo : IAccChart
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccChartRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(AccChartClass model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            var allData = await _context.AccChartClass.Where(x => x.ACCName == model.ACCName || x.ClassId == model.ClassId).ToListAsync();
            if (allData.Count > 0)
            {
                status.StatusCode = 0;
                status.Message = "Record already exists";
                return status;
            }

            model.ACCId = 0;
            model.CUser = currentUser.FullName;
            _context.AccChartClass.Add(model);
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

        public async Task<AccChartClass> GetById(int id)
        {
            var role = await _context.AccChartClass.Where(x => x.ACCId == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> Update(AccChartClass model)
        {
            var status = new ResponseStatus();
            var data = await _context.AccChartClass.Where(x => x.ACCId == model.ACCId).FirstOrDefaultAsync();

            data.ClassId = model.ClassId;
            data.ACCName = model.ACCName;
            data.ACCCType = model.ACCCType;
            _context.AccChartClass.Update(data);

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

            var assMaster = await _context.AccChartType.Where(x => x.ACCId == id).FirstOrDefaultAsync();
            if (assMaster != null)
            {
                status.StatusCode = 0;
                status.Message = "Can't Delete Has a DB Relation with Acc Chart Type";
                return status;
            }

            var data = await _context.AccChartClass.Where(x => x.ACCId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.AccChartClass.Remove(data);
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
        public async Task<List<AccChartClass>> GetAll()
        {
            var roles = await _context.AccChartClass.ToListAsync();
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
