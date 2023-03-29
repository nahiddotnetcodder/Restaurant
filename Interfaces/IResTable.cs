
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IResTable
    {
        PaginatedList<ResTable> GetItems(int pageIndex = 1, int pageSize = 10);
        Task<ResponseStatus> Create(ResTable model);
        Task<ResponseStatus> Update(ResTable model);
        Task<ResponseStatus> Delete(int id);
        Task<ResTable> GetById(int id);
        Task<List<ResTable>> GetAll();
        public bool IsItemExists(string number);
        public bool IsItemExists(string number, int id);
    }
}

