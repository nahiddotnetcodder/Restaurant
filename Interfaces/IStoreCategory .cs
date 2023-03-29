
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreCategory
    {
        PaginatedList<StoreCategory> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(StoreCategory model);
        Task<ResponseStatus> Update(StoreCategory model);
        Task<ResponseStatus> Delete(int id);
        Task<StoreCategory> GetById(int id);
        Task<List<StoreCategory>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

