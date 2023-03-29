
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreSCategory
    {
        PaginatedList<StoreSCategory> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(StoreSCategory model);
        Task<ResponseStatus> Update(StoreSCategory model);
        Task<ResponseStatus> Delete(int id);
        Task<StoreSCategory> GetById(int id);
        Task<List<StoreSCategory>> GetAll();
        public bool IsItemExists(int? id);
        public bool IsItemExists(string name, int id);
    }
}

