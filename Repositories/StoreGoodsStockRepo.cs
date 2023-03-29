using RMS.Models;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class StoreGoodsStockRepo : IStoreGoodsStock
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        public StoreGoodsStockRepo(RmsDbContext context) // will be passed by dependency injection.
        {
            _context = context; 
        }


        public StoreGoodsStock Create(StoreGoodsStock storeGoodsStock)
        {
            _context.StoreGoodsStock.Add(storeGoodsStock);
            _context.SaveChanges();
            return storeGoodsStock;
        }
        public StoreGoodsStock Edit(StoreGoodsStock storeGoodsStock)
        {
            _context.StoreGoodsStock.Attach(storeGoodsStock);
            _context.Entry(storeGoodsStock).State = EntityState.Modified;
            _context.SaveChanges();
            return storeGoodsStock;
        }
        private List<StoreGoodsStock> DoSort(List<StoreGoodsStock> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "SGSId")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.SGSId).ToList();
                else
                    items = items.OrderByDescending(n => n.SGSId).ToList();
            }
            else if (SortProperty.ToLower() == "code")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.ItemCode).ToList();
                else
                    items = items.OrderByDescending(n => n.ItemCode).ToList();
            }
            else if (SortProperty.ToLower() == "ItemDescription")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.ItemName).ToList();
                else
                    items = items.OrderByDescending(n => n.ItemName).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.SGSUPrice).ToList();
                else
                    items = items.OrderByDescending(d => d.SGSUPrice).ToList();
            }
            return items;
        }
        PaginatedList<StoreGoodsStock> IStoreGoodsStock.GetItems(string SortProperty, SortOrder sortOrder, string SearchText, int pageIndex, int pageSize)
        {
            List<StoreGoodsStock> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.StoreGoodsStock.Where(n => n.ItemName.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.StoreGoodsStock.ToList();
                items = DoSort(items, SortProperty, sortOrder);
                PaginatedList<StoreGoodsStock> retItems = new PaginatedList<StoreGoodsStock>(items, pageIndex, pageSize);
                return retItems;
        }

        StoreGoodsStock IStoreGoodsStock.GetItem(int SGSId)
        {
            StoreGoodsStock item = _context.StoreGoodsStock.Where(u => u.SGSId ==  SGSId)
                .FirstOrDefault();
            return item;
        }

        public StoreGoodsStock Delete(StoreGoodsStock storeGoodsStock)
        {
            _context.StoreGoodsStock.Attach(storeGoodsStock);
            _context.Entry(storeGoodsStock).State = EntityState.Deleted;
            _context.SaveChanges();
            return storeGoodsStock;
        }

        public async Task<List<StoreGoodsStock>> GetAll()
        {
            var roles = await _context.StoreGoodsStock.ToListAsync();
            return roles;
        }
    }
}
