
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRWeekend
    {
        PaginatedList<HRWeekend> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(HRWeekend model);
        Task<ResponseStatus> Update(HRWeekend model);
        Task<ResponseStatus> Delete(int id);
        Task<HRWeekend> GetById(int id);
        Task<List<HRWeekend>> GetAll();
        public bool IsItemExists(int id);
        public bool IsItemExists(int name, int id);
    }
}

