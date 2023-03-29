
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IResKitchenInfo
    {
        PaginatedList<ResKitchenInfo> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(ResKitchenInfo model);
        Task<ResponseStatus> Update(ResKitchenInfo model);
        Task<ResponseStatus> Delete(int id);
        Task<ResKitchenInfo> GetById(int id);
        Task<List<ResKitchenInfo>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

