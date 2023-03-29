using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class AccBankAccountsRepo : IAccBankAccounts
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccBankAccountsRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<ResponseStatus> Create(AccBankAccounts model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            model.ABAId = 0;
            model.CUser = currentUser.FullName;
            _context.AccBankAccounts.Add(model);
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
        public async Task<List<AccBankAccounts>> GetAll()
        {
            var roles = await _context.AccBankAccounts.ToListAsync();
            return roles;

            //var result = new List<AccBankAccounts>();
            //var data = await _context.AccBankAccounts.ToListAsync();
            //if (data == null)
            //    return result;
            //foreach (var item in data)
            //{
            //    result.Add(new AccBankAccounts
            //    {
            //        ABAId = item.ABAId,
            //        ACMAccCode = item.ACMAccCode,
            //        ABAAType = item.ABAAType,
            //        ABABAName = item.ABABAName,
            //        ABABANumber = item.ABABANumber,
            //        ABABName = item.ABABName,
            //        ABABAdd = item.ABABAdd,
            //        ABABCCode = item.ABABCCode,
            //        ABALRDate = item.ABALRDate,
            //        ABAERBal = item.ABAERBal,
            //        IsActive = item.IsActive,
            //        CUser = item.CUser
            //    });
            //}
            //return result;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }
        public async Task<AccBankAccounts> GetById(int id)
        {
            var role = await _context.AccBankAccounts.Where(x => x.ABAId == id).FirstOrDefaultAsync();
            return role;
        }
       
        public async Task<ResponseStatus> DeleteItemById(int id)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var data = await _context.AccBankAccounts.Where(x => x.ABAId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.AccBankAccounts.Remove(data);
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
        public async Task<ResponseStatus> Update(AccBankAccounts model)
        {
            var status = new ResponseStatus();
            var data = await _context.AccBankAccounts.Where(x => x.ABAId == model.ABAId).FirstOrDefaultAsync();
            data.ABAId = model.ABAId;
            data.ACMAccCode = model.ACMAccCode;
            data.ABAAType = model.ABAAType;
            data.ABABAName = model.ABABAName;
            data.ABABANumber = model.ABABANumber;
            data.ABABName = model.ABABName;
            data.ABABAdd = model.ABABAdd;
            data.ABABCCode = model.ABABCCode;
            data.IsActive = model.IsActive;
            _context.AccBankAccounts.Update(data);

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
    }
}
