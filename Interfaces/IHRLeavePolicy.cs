
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRLeavePolicy
    {
        PaginatedList<HRLeavePolicy> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(HRLeavePolicy model);
        Task<ResponseStatus> Update(HRLeavePolicy model);
        Task<ResponseStatus> Delete(int id);
        Task<HRLeavePolicy> GetById(int id);
        Task<List<HRLeavePolicy>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

