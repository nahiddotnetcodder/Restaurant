
using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreGoodsStock
    {
        PaginatedList<StoreGoodsStock> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        StoreGoodsStock GetItem(int SGSId); // read particular item
        StoreGoodsStock Create(StoreGoodsStock storeGoodsStock);
        StoreGoodsStock Edit(StoreGoodsStock storeGoodsStock);
        StoreGoodsStock Delete(StoreGoodsStock storeGoodsStock);
        Task<List<StoreGoodsStock>> GetAll();
        //void UpdateStock(StoreGReceive receive);
    }
}

