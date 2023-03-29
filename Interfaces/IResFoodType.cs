
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IResFoodType
    {
        PaginatedList<ResFoodType> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(ResFoodType model);
        Task<ResponseStatus> Update(ResFoodType model);
        Task<ResponseStatus> Delete(int id);
        Task<ResFoodType> GetById(int id);
        Task<List<ResFoodType>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

