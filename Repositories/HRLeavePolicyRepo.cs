using Newtonsoft.Json;
using System.Threading.Tasks;
public class HRLeavePolicyRepo : IHRLeavePolicy
{
    private readonly RmsDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HRLeavePolicyRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseStatus> Create(HRLeavePolicy model)
    {
        var status = new ResponseStatus();

        var currentUser = GetCurrentUser();
        var allData = await _context.HRLeavePolicy.Where(x => x.HRLPName == model.HRLPName || x.HRLPId == model.HRLPId).ToListAsync();
        if (allData.Count > 0)
        {
            status.StatusCode = 0;
            status.Message = "Record already exists";
            return status;
        }

        model.HRLPId = 0;
        model.CUser = currentUser.Id;
        _context.HRLeavePolicy.Add(model);
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

    public async Task<HRLeavePolicy> GetById(int id)
    {
        var role = await _context.HRLeavePolicy.Where(x => x.HRLPId == id).FirstOrDefaultAsync();
        return role;
    }

    public async Task<ResponseStatus> Update(HRLeavePolicy model)
    {
        var status = new ResponseStatus();
        var allData = await _context.HRLeavePolicy.Where(x => x.HRLPName == model.HRLPName ).ToListAsync();
        if (allData.Count > 0)
        {
            status.StatusCode = 0;
            status.Message = "Record already exists";
            return status;
        }
        var data = await _context.HRLeavePolicy.Where(x => x.HRLPId == model.HRLPId).FirstOrDefaultAsync();

        data.HRLPId = model.HRLPId;
        data.HRLPName = model.HRLPName;
        data.HRLPTDay = model.HRLPTDay;
        _context.HRLeavePolicy.Update(data);

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
        var data = await _context.HRLeavePolicy.Where(x => x.HRLPId == id).FirstOrDefaultAsync();

        if (data == null)
        {
            status.StatusCode = 0;
            status.Message = "Entity Updation Failed";
            return status;
        }
        _context.HRLeavePolicy.Remove(data);
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
    public async Task<List<HRLeavePolicy>> GetAll()
    {
        var roles = await _context.HRLeavePolicy.ToListAsync();
        return roles;
    }
    public ApplicationUser GetCurrentUser()
    {
        var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
        var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
        return currentUser;
    }

    public PaginatedList<HRLeavePolicy> GetItems(int pageIndex = 1, int pageSize = 10)
    {
        List<HRLeavePolicy> items;
        items = _context.HRLeavePolicy.ToList();
        PaginatedList<HRLeavePolicy> retItems = new PaginatedList<HRLeavePolicy>(items, pageIndex, pageSize);
        return retItems;
    }

    public bool IsItemExists(string name)
    {
        int ct = _context.HRLeavePolicy.Where(n => n.HRLPName.ToLower() == name.ToLower()).Count();
        if (ct > 0)
            return true;
        else
            return false;
    }

    public bool IsItemExists(string name, int id)
    {
        int ct = _context.HRLeavePolicy.Where(n => n.HRLPName.ToLower() == name.ToLower() && n.HRLPId != id).Count();
        if (ct > 0)
            return true;
        else
            return false;
    }
}