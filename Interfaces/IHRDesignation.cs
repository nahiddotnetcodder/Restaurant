
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRDesignation
    {
        PaginatedList<HRDesignation> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(HRDesignation model);
        Task<ResponseStatus> Update(HRDesignation model);
        Task<ResponseStatus> Delete(int id);
        Task<HRDesignation> GetById(int id);
        Task<List<HRDesignation>> GetAll();
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);
    }
}

