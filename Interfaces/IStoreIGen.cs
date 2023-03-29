
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreIGen
    {
        PaginatedList<StoreIGen> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        StoreIGen GetItem(int rdcid); // read particular item
        StoreIGen Create(StoreIGen storeIGen);
        StoreIGen Edit(StoreIGen storeIGen);
        StoreIGen Delete(StoreIGen storeIGen);
        public bool IsItemExists(string name);
        public bool IsItemExists(string code, int id);
        Task<List<StoreIGen>> GetAll();
    }
}

