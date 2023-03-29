using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreGReceiveMasterRepo : IStoreGReceiveMaster
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreGReceiveMasterRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
       
        public async Task<ResponseStatus> Create(StoreGReceiveMaster model)
        {
            foreach (var item in model.StoreGReceiveDetails)
            {
                var exiting = _context.StoreGoodsStock.FirstOrDefault(n => n.ItemCode == item.ItemCode);         
                if (exiting != null)
                {
                    exiting.SGSQty += item.GRDQty;
                    exiting.Unit = item.Unit;
                    //exiting.SGSUPrice += item.GRDUPrice ;
                    exiting.SGSTPrice += item.GRDTPrice;
                    exiting.SGSUPrice = exiting.SGSTPrice / exiting.SGSQty;
                   
                    _context.SaveChanges();
                    
                }
                else
                {
                    var newRecord = new StoreGoodsStock { ItemCode = item.ItemCode, ItemName = item.ItemName,Unit = item.Unit, SGSQty = item.GRDQty, SGSUPrice = item.GRDUPrice, SGSTPrice = item.GRDTPrice };
                    _context.StoreGoodsStock.Add(newRecord);
                    _context.SaveChanges();
                }
            }


            var status = new ResponseStatus();
            var currentUser = GetCurrentUser();

            model.GRMId = 0;
            model.CUser = currentUser.FullName;
            _context.StoreGReceiveMasters.Add(model);
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
        public async Task<List<StoreGReceiveMaster>> GetAll()
        {
            var result = new List<StoreGReceiveMaster>();
            var data = await _context.StoreGReceiveMasters.Include(x=>x.StoreSuppliers).ToListAsync();
            if (data == null)
                return result;
            foreach (var item in data)
            {
                result.Add(new StoreGReceiveMaster
                {
                    GRMId = item.GRMId,
                    GRMDate = item.GRMDate,
                    StoreSuppliersName = item.StoreSuppliers == null ? string.Empty : item.StoreSuppliers.SSName,
                    GRMRemarks = item.GRMRemarks,
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
        public async Task<StoreGReceiveMaster> GetById(int id)
        {
            var data = from master in _context.StoreGReceiveMasters.Where(x => x.GRMId == id).Include(x => x.StoreGReceiveDetails)
                       select new StoreGReceiveMaster
                       {
                           GRMId = master.GRMId,
                           GRMDate = master.GRMDate,
                           SSId = master.SSId,
                           StoreSuppliersName = master.StoreSuppliers == null ? string.Empty : master.StoreSuppliers.SSName,
                           GRMRemarks = master.GRMRemarks,
                           CUser = master.CUser,
                           StoreGReceiveDetails = master.StoreGReceiveDetails
                       };

            var dbData = await data.FirstOrDefaultAsync();
            var result = new StoreGReceiveMaster
            {
                GRMId = dbData.GRMId,
                GRMDate = dbData.GRMDate,
                SSId = dbData.SSId,
                StoreSuppliersName = dbData.StoreSuppliersName,
                GRMRemarks = dbData.GRMRemarks,
                //CUser = dbData.CUser,
                StoreGReceiveDetails = GenerateItemsViewModel(dbData)
            };
            return result;
        }
        private List<StoreGReceiveDetails> GenerateItemsViewModel(StoreGReceiveMaster model)
        {
            var result = new List<StoreGReceiveDetails>();
            if (model.StoreGReceiveDetails == null)
                return result;
            foreach (var item in model.StoreGReceiveDetails)
            {
                result.Add(new StoreGReceiveDetails
                {
                    GRDId = item.GRDId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    Unit = item.Unit,
                    GRDQty = item.GRDQty,
                    GRDUPrice = item.GRDUPrice,
                    GRDTPrice = item.GRDTPrice
                });
            }
            return result;
        }
        public async Task<ResponseStatus> DeleteItemById(int id)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var glItem = await _context.StoreGReceiveDetails.Where(x => x.GRDId == id).FirstOrDefaultAsync();
            if (glItem == null)
            {
                status.StatusCode = 0;
                status.Message = "Item Not Found";
                return status;
            }
            else
            {
                _context.StoreGReceiveDetails.Remove(glItem);
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
        public async Task<ResponseStatus> Update(StoreGReceiveMaster model)
        {
            var status = new ResponseStatus();
            var result = 0;
            var existingParent = _context.StoreGReceiveMasters.Where(p => p.GRMId == model.GRMId).Include(p => p.StoreGReceiveDetails).SingleOrDefault();
            if (existingParent != null)
            {
                model.CreateDate = existingParent.CreateDate;
                // Update parent
                _context.Entry(existingParent).CurrentValues.SetValues(model);
                result = await _context.SaveChangesAsync();

                //Delete children
                foreach (var existingChild in existingParent.StoreGReceiveDetails.ToList())
                {
                    if (model.StoreGReceiveDetails != null && model.StoreGReceiveDetails.Any())
                    {
                        if (!model.StoreGReceiveDetails.Any(c => c.GRDId == existingChild.GRDId))
                            _context.StoreGReceiveDetails.Remove(existingChild);
                    }
                }

                if (model.StoreGReceiveDetails != null && model.StoreGReceiveDetails.Any())
                {
                    foreach (var childModel in model.StoreGReceiveDetails)
                    {
                        var existingChild = existingParent.StoreGReceiveDetails.Where(c => c.GRDId == childModel.GRDId).FirstOrDefault();

                        if (existingChild != null && existingChild.GRDId > 0)
                        // Update child
                        {
                            childModel.GRMId = model.GRMId;
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                        }
                        else
                        {
                            // Insert child
                            childModel.StoreGReceiveMaster = model;
                            existingParent.StoreGReceiveDetails.Add(childModel);
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
            var data = await _context.StoreGReceiveMasters.Where(x => x.GRMId == id).FirstOrDefaultAsync();

            if (data == null)
            {
                status.StatusCode = 0;
                status.Message = "Entity Updation Failed";
                return status;
            }
            _context.StoreGReceiveMasters.Remove(data);
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
