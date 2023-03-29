
using DevExpress.Xpo;
using DevExpress.XtraEditors.Filtering;
using Newtonsoft.Json;
using RMS.Models;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreSuppliersRepo : IStoreSuppliers
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreSuppliersRepo(RmsDbContext context, IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor) // will be passed by dependency injection.
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public StoreSuppliers Create(StoreSuppliers model)
        {
            var currentUser = GetCurrentUser();
            model.CUser = currentUser.Id;

            _context.StoreSupplier.Add(model);
            _context.SaveChanges();
            return model;
        }
    public StoreSuppliers Delete(StoreSuppliers model)
        {
            _context.StoreSupplier.Attach(model);
            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
            return model;
        }
        public StoreSuppliers Edit(StoreSuppliers model)
        {
            var currentUser = GetCurrentUser();
            model.CUser = currentUser.Id;

            _context.StoreSupplier.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            return model;
        }
        
        public StoreSuppliers GetItem(int Id)
        {
            StoreSuppliers item = _context.StoreSupplier.Where(u => u.SSId == Id)
                .FirstOrDefault();
            return item;
        }
       
        public bool IsEmpCodeExists(int Id)
        { 
            int ct = _context.StoreSupplier.Where(n => n.SSId == Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsEmpCodeExists(string name,int Id)
        {
            int ct = _context.StoreSupplier.Where(n => n.SSName.ToLower() == name.ToLower() && n.SSId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        //auto Genarate EmpID
        public string GetEmpID()
        {

            string empId = "";
            var lastEmpId = _context.HREmpDetails.Max(n => n.HREDEId);

            if (lastEmpId == null)
            {
                empId = "EM0001";
            }
            else
            {
                int lastdigit = 1;
                int.TryParse(lastEmpId.Substring(2, 4).ToString(), out lastdigit);
                empId = "EM" + (lastdigit + 1).ToString().PadLeft(4, '0');
            }
            return empId;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }

        public PaginatedList<StoreSuppliers> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<StoreSuppliers> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.StoreSupplier.Where(n => n.SSName.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.StoreSupplier.ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<StoreSuppliers> retItems = new PaginatedList<StoreSuppliers>(items, pageIndex, pageSize);
            return retItems;
        }

        private List<StoreSuppliers> DoSort(List<StoreSuppliers> items, string SortProperty, SortOrder sortOrder)
        {
            if (SortProperty.ToLower() == "Name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.SSName).ToList();
                else
                    items = items.OrderByDescending(n => n.SSName).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.SSSName).ToList();
                else
                    items = items.OrderByDescending(d => d.SSSName).ToList();
            }
            return items;
        }
        public async Task<List<StoreSuppliers>> GetAll()
        {
            var roles = await _context.StoreSupplier.ToListAsync();
            return roles;
        }
    }
}
