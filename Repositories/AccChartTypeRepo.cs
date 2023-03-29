using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class AccChartTypeRepo : IAccChartType
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccChartTypeRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(AccChartType model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            if (model.ACCId == -1)
                model.ACCId = null;
            if (model.ACTParentName == null)
                model.ACTParentName = string.Empty;
            model.ACTId = 0;
            model.CUser = currentUser.Id;
            _context.AccChartType.Add(model);
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

        public async Task<AccChartType> GetById(int id)
        {
            var data = await _context.AccChartType.Include(ch => ch.AccChartClasss).Where(x=> x.ACTId == id).ToListAsync();
            var result = data.Select(x => new AccChartType
            {
                ACTId = x.ACTId,
                ACTClassId = x.ACTClassId,
                ACTName = x.ACTName,
                ACCId = x.ACCId,
                ACTParentId = x.ACTParentId,
                ACTParentName = x.ACTParentName
            }).FirstOrDefault();
            return result;
        }

        public async Task<ResponseStatus> Update(AccChartType model)
        {
            var status = new ResponseStatus();
            var data = await _context.AccChartType.Where(x => x.ACTId == model.ACTId).FirstOrDefaultAsync();

            if (model.ACCId == -1)
                model.ACCId = null;
            data.ACTName = model.ACTName;
            data.ACCId = model.ACCId;
            data.ACTParentId = model.ACTParentId;
            data.ACTParentName = model.ACTParentName;
            _context.AccChartType.Update(data);

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

            var assMaster = await _context.AccChartMaster.Where(x => x.ACTId == id).FirstOrDefaultAsync();
            if(assMaster != null)
            {
                status.StatusCode = 0;
                status.Message = "Can't Delete Has a DB Relation with Acc Master";
                return status;
            }

            var data = await _context.AccChartType.Where(x => x.ACTId == id).FirstOrDefaultAsync();

           if(data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.AccChartType.Remove(data);
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
        public async Task<List<AccChartType>> GetAll()
        {
            var data = await _context.AccChartType.Include(x=> x.AccChartClasss).ToListAsync();
            var result = data.Select(x => new AccChartType
            {
                ACTId = x.ACTId,
                ACTClassId = x.ACTClassId,
                ACTName = x.ACTName,
                ACCId = x.ACCId,
                AccChartClassName = x.AccChartClasss == null ? string.Empty : x.AccChartClasss.ACCName,
                ACTParentId = x.ACTParentId,
                ACTParentName = x.ACTParentName
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
