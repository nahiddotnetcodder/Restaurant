using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IMenu
    {
        Task<ResponseStatus> Create(Menu model);
        Task<ResponseStatus> Update(Menu model);
        Task<List<Menu>> GetAll();
        Task<Menu> GetById(int id);
        Task<ResponseStatus> Delete(int id);
    }
}

