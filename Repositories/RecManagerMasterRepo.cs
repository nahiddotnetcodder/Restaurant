using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class RecManagerMasterRepo : IRecManagerMaster
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RecManagerMasterRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
       
        public async Task<ResponseStatus> Create(RMMaster model)
        {
            var status = new ResponseStatus();
            var currentUser = GetCurrentUser();

            model.RMMId = 0;
            model.CUser = currentUser.FullName;
            _context.RMMaster.Add(model);
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
        public async Task<List<RMMaster>> GetAll()
        {
            var result = new List<RMMaster>();
            var data = await _context.RMMaster.ToListAsync();
            if (data == null)
                return result;
            foreach (var item in data)
            {
                result.Add(new RMMaster
                {
                    RMMId = item.RMMId,
                    RMItemCode = item.RMItemCode,
                    RMItemName = item.RMItemName,
                    CUser = item.CUser
                });
            }
            return result;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }
        public async Task<RMMaster> GetById(int id)
        {
            var data = from master in _context.RMMaster.Where(x => x.RMMId == id).Include(x => x.RMDetails)
                       select new RMMaster
                       {
                           RMMId = master.RMMId,
                           RMItemCode = master.RMItemCode,
                           RMItemName = master.RMItemName,
                           CUser = master.CUser,
                           RMDetails = master.RMDetails
                       };

            var dbData = await data.FirstOrDefaultAsync();
            var result = new RMMaster
            {
                RMMId = dbData.RMMId,
                RMItemCode = dbData.RMItemCode,
                RMItemName = dbData.RMItemName,
                CUser = dbData.CUser,
                RMDetails = GenerateItemsViewModel(dbData)
            };
            return result;
        }
        private List<RMDetails> GenerateItemsViewModel(RMMaster model)
        {
            var result = new List<RMDetails>();
            if (model.RMDetails == null)
                return result;
            foreach (var item in model.RMDetails)
            {
                result.Add(new RMDetails
                {
                    RMDId = item.RMDId,
                    SIGItemCode = item.SIGItemCode,
                    SIGItemName = item.SIGItemName,
                    RMDQty = item.RMDQty,
                    SIGUnit = item.SIGUnit,
                    SGSUPrice = item.SGSUPrice
                });
            }
            return result;
        }
        public async Task<ResponseStatus> DeleteItemById(int id)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var glItem = await _context.RMDetails.Where(x => x.RMDId == id).FirstOrDefaultAsync();
            if (glItem == null)
            {
                status.StatusCode = 0;
                status.Message = "Item Not Found";
                return status;
            }
            else
            {
                _context.RMDetails.Remove(glItem);
                var result = await _context.SaveChangesAsync();
                if (result < 1)
                {
                    status.StatusCode = 0;
                    status.Message = "Item deletion Failed";
                    return status;
                }
                else
                {
                    status.StatusCode = 1;
                    status.Message = "Deleted Successfully";
                    return status;
                }
            }
        }
        public async Task<ResponseStatus> Update(RMMaster model)
        {
            var status = new ResponseStatus();
            var result = 0;
            var existingParent = _context.RMMaster.Where(p => p.RMMId == model.RMMId).Include(p => p.RMDetails).SingleOrDefault();
            if (existingParent != null)
            {
                model.CreateDate = existingParent.CreateDate;
                // Update parent
                _context.Entry(existingParent).CurrentValues.SetValues(model);
                result = await _context.SaveChangesAsync();

                //Delete children
                foreach (var existingChild in existingParent.RMDetails.ToList())
                {
                    if (model.RMDetails != null && model.RMDetails.Any())
                    {
                        if (!model.RMDetails.Any(c => c.RMDId == existingChild.RMDId))
                            _context.RMDetails.Remove(existingChild);
                    }
                }

                if (model.RMDetails != null && model.RMDetails.Any())
                {
                    foreach (var childModel in model.RMDetails)
                    {
                        var existingChild = existingParent.RMDetails.Where(c => c.RMDId == childModel.RMDId).FirstOrDefault();

                        if (existingChild != null && existingChild.RMDId > 0)
                        // Update child
                        {
                            childModel.RMMId = model.RMMId;
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                        }
                        else
                        {
                            // Insert child
                            childModel.RMMaster = model;
                            existingParent.RMDetails.Add(childModel);
                        }
                    }
                    result = await _context.SaveChangesAsync();
                }
            }
            if (result < 1)
            {
                status.StatusCode = 0;
                status.Message = "Update Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "Updated successfully";
            return status;
        }

        public async Task<ResponseStatus> Delete(int id)
        {
            var status = new ResponseStatus();
            var data = await _context.RMMaster.Where(x => x.RMMId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.RMMaster.Remove(data);
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
