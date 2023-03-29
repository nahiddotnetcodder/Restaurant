
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRDepartment
    {
        PaginatedList<HRDepartment> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(HRDepartment model);
        Task<ResponseStatus> Update(HRDepartment model);
        Task<ResponseStatus> Delete(int id);
        Task<HRDepartment> GetById(int id);
        Task<List<HRDepartment>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

