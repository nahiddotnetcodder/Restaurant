
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHREmpRoaster
    {
        PaginatedList<HREmpRoaster> GetItems(int pageIndex = 1, int pageSize = 10); //read all
        Task<ResponseStatus> Create(HREmpRoaster model);
        Task<ResponseStatus> Update(HREmpRoaster model);
        Task<ResponseStatus> Delete(int id);
        Task<HREmpRoaster> GetById(int id);
        Task<List<HREmpRoaster>> GetAll();
        public bool IsItemExists(int ? id);
        public bool IsItemExists(string name, int id);
    }
}

