
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreUnit
    {
        PaginatedList<StoreUnit> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(StoreUnit model);
        Task<ResponseStatus> Update(StoreUnit model);
        Task<ResponseStatus> Delete(int id);
        Task<StoreUnit> GetById(int id);
        Task<List<StoreUnit>> GetAll();
        public bool IsItemExists(string name, int id);
    }
}
