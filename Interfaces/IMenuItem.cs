using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IMenuItem
    {
        Task<ResponseStatus> Create(MenuItem model);
        Task<ResponseStatus> Update(MenuItem model);
        Task<ResponseStatus> Delete(int id);
        Task<MenuItem> GetById(int id);
        Task<List<MenuItem>> GetAll();
    }
}

