using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class AccJournalEntryRepo : IAccJournalEntry
    {
        private readonly RmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccJournalEntryRepo(RmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetRefPrefixNo()
        {
            var data = await _context.AccJournal.OrderBy(x => x.AJId).LastOrDefaultAsync();
            if (data == null)
                return 1.ToString().PadLeft(4, '0');
            int serial = Convert.ToInt32(data.AJRefPrefix);
            serial += 1;
            return serial.ToString().PadLeft(4, '0');
        }
        public async Task<ResponseStatus> Create(AccJournal model)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();

            model.AJId = 0;
            model.CUser = currentUser.FullName;
            model.AJType = "Journal Entry";
            var debitAmount = model.AccGlTrans.Sum(x => x.AGTDebitAccount);
            var creditAmount = model.AccGlTrans.Sum(x => x.AGTCreditAccount);
            model.AJAmount = (creditAmount ?? 0) - (debitAmount ?? 0);
            _context.AccJournal.Add(model);
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
        public async Task<List<AccJournalViewModel>> GetAll()
        {
            var result = new List<AccJournalViewModel>();
            var data = await _context.AccJournal.ToListAsync();
            if (data == null)
                return result;
            foreach (var item in data)
            {
                result.Add(new AccJournalViewModel
                {
                    AJId = item.AJId,
                    AJTrDate = item.AJTrDate,
                    AJRef = item.AJRef,
                    JournalType = item.AJType,
                    AJAmount = item.AJAmount,
                    AJMemo = item.AJMemo,
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
        public async Task<AccJournalViewModel> GetById(int id)
        {
            var data = from journal in _context.AccJournal.Where(x => x.AJId == id).Include(x=> x.AccGlTrans)
                       select new AccJournal
                       {
                           AJId = journal.AJId,
                           AJTrDate = journal.AJTrDate,
                           AJDDate = journal.AJDDate,
                           AJEDate = journal.AJEDate,
                           AJRef = journal.AJRef,
                           AJSoRef = journal.AJSoRef,
                           AJType = journal.AJType,
                           AJAmount = journal.AJAmount,
                           AJMemo = journal.AJMemo,
                           CUser = journal.CUser,
                           AccGlTrans = journal.AccGlTrans
                       };

            var dbData = await data.FirstOrDefaultAsync();
            var result = new AccJournalViewModel{
                AJId = dbData.AJId,
                AJTrDate = dbData.AJTrDate,
                AJDDate = dbData.AJDDate,
                AJEDate = dbData.AJEDate,
                AJRef = dbData.AJRef,
                AJSoRef= dbData.AJSoRef,
                JournalType = dbData.AJType,
                AJAmount = dbData.AJAmount,
                AJMemo = dbData.AJMemo,
                CUser = dbData.CUser,
                Items = GenerateItemsViewModel(dbData)
            };
            return result;
        }
        private List<AccGlTransViewModel> GenerateItemsViewModel(AccJournal model)
        {
            var result = new List<AccGlTransViewModel>();
            if (model.AccGlTrans == null)
                return result;
            foreach(var item in model.AccGlTrans)
            {
                result.Add(new AccGlTransViewModel
                {
                    AGTId = item.AGTId,
                    AJTrDate = model.AJTrDate,
                    AJTrNo = item.AJTrNo,
                    AGTAccCode = item.AGTAccCode,
                    AGTAccDescription = item.AGTAccDescription,
                    AGTDebitAccount = item.AGTDebitAccount ?? 0,
                    AGTCreditAccount = item.AGTCreditAccount ?? 0,
                    AGTMemo = item.AGTMemo == null ? string.Empty : item.AGTMemo,
                });
            }
            return result;
        }
        public async Task<ResponseStatus> DeleteItemById(int id)
        {
            var status = new ResponseStatus();

            var currentUser = GetCurrentUser();
            var glItem = await _context.AccGlTrans.Where(x => x.AGTId == id).FirstOrDefaultAsync();
            if(glItem == null)
            {
                status.StatusCode = 0;
                status.Message = "Item Not Found";
                return status;
            }
            else
            {
                _context.AccGlTrans.Remove(glItem);
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
        public async Task<ResponseStatus> Update(AccJournal model)
        {
            var status = new ResponseStatus();
            var result = 0;
            var existingParent = _context.AccJournal.Where(p => p.AJId == model.AJId).Include(p => p.AccGlTrans).SingleOrDefault();
            if (existingParent != null)
            {
                var debitAmount = model.AccGlTrans.Sum(x => x.AGTDebitAccount);
                var creditAmount = model.AccGlTrans.Sum(x => x.AGTCreditAccount);
                model.AJAmount = existingParent.AJAmount + (creditAmount ?? 0) - (debitAmount ?? 0);
                model.CUser = existingParent.CUser;
                model.AJType = existingParent.AJType;
                model.AJRefPrefix = existingParent.AJRefPrefix;
                model.CreateDate = existingParent.CreateDate;
                // Update parent
                _context.Entry(existingParent).CurrentValues.SetValues(model);
                result = await _context.SaveChangesAsync();

                //Delete children
                foreach (var existingChild in existingParent.AccGlTrans.ToList())
                {
                    if (model.AccGlTrans != null && model.AccGlTrans.Any())
                    {
                        if (!model.AccGlTrans.Any(c => c.AGTId == existingChild.AGTId))
                            _context.AccGlTrans.Remove(existingChild);
                    }
                }

                if (model.AccGlTrans != null && model.AccGlTrans.Any())
                {
                    foreach (var childModel in model.AccGlTrans)
                    {
                        var existingChild = existingParent.AccGlTrans.Where(c => c.AGTId == childModel.AGTId).FirstOrDefault();

                        if (existingChild != null && existingChild.AGTId > 0)
                        // Update child
                        {
                            childModel.AJTrNo = model.AJId;
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                        }
                        else
                        {
                            // Insert child
                            childModel.AccJournal = model;
                            existingParent.AccGlTrans.Add(childModel);
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
    }
}
