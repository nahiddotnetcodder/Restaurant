using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreGIssueMasterRepo : IStoreGIssueMaster
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreGIssueMasterRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> Create(StoreGIssueMaster model)
        {
           
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            model.GIMId = 0;
            model.CUser = currentUser.FullName;

            foreach (var item in model.StoreGIssueDetails)
            {
                var exiting = _context.StoreGoodsStock.FirstOrDefault(n => n.ItemCode == item.ItemCode);
                if (exiting != null)
                {
                    if (item.GIDQty <= exiting.SGSQty)
                    {
                        exiting.SGSQty -= item.GIDQty;
                        //exiting.SGSUPrice -= item.GIDUPrice;
                        _context.SaveChanges();
                    }
                    else
                    {
                        status.StatusCode = 0;
                        status.Message = "Stock Out";
                        return status;
                    }

                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Not Found";
                    return status;
                }
            }

            _context.StoreGIssueMasters.Add(model);
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
        public async Task<List<StoreGIssueMaster>> GetAll()
        {
            var result = new List<StoreGIssueMaster>();
            var data = await _context.StoreGIssueMasters.Include(x => x.HRDepart).ToListAsync();
            if (data == null)
                return result;
            foreach (var item in data)
            {
                result.Add(new StoreGIssueMaster
                {
                    GIMId = item.GIMId,
                    GIMDate = item.GIMDate,
                    HRDepartName = item.HRDepart == null ? string.Empty : item.HRDepart.HRDName,
                    GIMRemarks = item.GIMRemarks,
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
        public async Task<StoreGIssueMaster> GetById(int id)
        {
            var data = from model in _context.StoreGIssueMasters.Where(x => x.GIMId == id).Include(x => x.StoreGIssueDetails)
                       select new StoreGIssueMaster
                       {
                           GIMId = model.GIMId,
                           GIMDate = model.GIMDate,
                           HRDId= model.HRDId,
                           HRDepartName = model.HRDepart == null ? string.Empty : model.HRDepart.HRDName,
                           GIMRemarks = model.GIMRemarks,
                           CUser = model.CUser,
                           StoreGIssueDetails = model.StoreGIssueDetails
                       };

            var dbData = await data.FirstOrDefaultAsync();
            var result = new StoreGIssueMaster
            {
                GIMId = dbData.GIMId,
                GIMDate = dbData.GIMDate,
                HRDId = dbData.HRDId,
                HRDepartName = dbData.HRDepartName,
                GIMRemarks = dbData.GIMRemarks,
                CUser = dbData.CUser,
                StoreGIssueDetails = GenerateItemsViewModel(dbData)
            };
            return result;
        }
        private List<StoreGIssueDetails> GenerateItemsViewModel(StoreGIssueMaster model)
        {
            var result = new List<StoreGIssueDetails>();
            if (model.StoreGIssueDetails == null)
                return result;
            foreach (var item in model.StoreGIssueDetails)
            {
                result.Add(new StoreGIssueDetails
                {
                    GIDId = item.GIDId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    Unit = item.Unit,
                    GIDQty = item.GIDQty,
                    GIDUPrice = item.GIDUPrice,
                    GIDTPrice = item.GIDTPrice
                });
            }
            return result;
        }
        public async Task<ResponseStatus> DeleteItemById(int id)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var glItem = await _context.StoreGIssueDetails.Where(x => x.GIDId == id).FirstOrDefaultAsync();
            if (glItem == null)
            {
                status.StatusCode = 0;
                status.Message = "Item Not Found";
                return status;
            }
            else
            {
                _context.StoreGIssueDetails.Remove(glItem);
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
        public async Task<ResponseStatus> Update(StoreGIssueMaster model)
        {
            var status = new ResponseStatus();
            var result = 0;
            var existingParent = _context.StoreGIssueMasters.Where(p => p.GIMId == model.GIMId).Include(p => p.HRDepart).SingleOrDefault();
            if (existingParent != null)
            {
                model.CreateDate = existingParent.CreateDate;
                // Update parent
                _context.Entry(existingParent).CurrentValues.SetValues(model);
                result = await _context.SaveChangesAsync();

                //Delete children
                foreach (var existingChild in existingParent.StoreGIssueDetails.ToList())
                {
                    if (model.StoreGIssueDetails != null && model.StoreGIssueDetails.Any())
                    {
                        if (!model.StoreGIssueDetails.Any(c => c.GIDId == existingChild.GIDId))
                            _context.StoreGIssueDetails.Remove(existingChild);
                    }
                }

                if (model.StoreGIssueDetails != null && model.StoreGIssueDetails.Any())
                {
                    foreach (var childModel in model.StoreGIssueDetails)
                    {
                        var existingChild = existingParent.StoreGIssueDetails.Where(c => c.GIDId == childModel.GIDId).FirstOrDefault();

                        if (existingChild != null && existingChild.GIDId > 0)
                        // Update child
                        {
                            childModel.GIMId = model.GIMId;
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                        }
                        else
                        {
                            // Insert child
                            childModel.StoreGIssueMaster = model;
                            existingParent.StoreGIssueDetails.Add(childModel);
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
            var data = await _context.StoreGIssueMasters.Where(x => x.GIMId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.StoreGIssueMasters.Remove(data);
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

        public DateTime getdate()
        {
            try
            {
                var lastDate = _context.StoreDClose.Max(n => n.SDCDate);
                return lastDate.AddDays(1);
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
