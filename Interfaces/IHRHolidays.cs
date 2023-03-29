
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRHolidays
    {
        PaginatedList<HRHolidays> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(HRHolidays model);
        Task<ResponseStatus> Update(HRHolidays model);
        Task<ResponseStatus> Delete(int id);
        Task<HRHolidays> GetById(int id);
        Task<List<HRHolidays>> GetAll();
        public bool IsItemExists(int? id);
        public bool IsItemExists(string name, int id);
    }
}

