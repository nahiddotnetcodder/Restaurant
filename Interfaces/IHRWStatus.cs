
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRWStatus
    {
        PaginatedList<HRWStatus> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(HRWStatus model);
        Task<ResponseStatus> Update(HRWStatus model);
        Task<ResponseStatus> Delete(int id);
        Task<HRWStatus> GetById(int id);
        Task<List<HRWStatus>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

