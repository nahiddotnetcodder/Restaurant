
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRSalaryPolicy
    {
        PaginatedList<HRSalaryPolicy> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(HRSalaryPolicy model);
        Task<ResponseStatus> Update(HRSalaryPolicy model);
        Task<ResponseStatus> Delete(int id);
        Task<HRSalaryPolicy> GetById(int id);
        Task<List<HRSalaryPolicy>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

