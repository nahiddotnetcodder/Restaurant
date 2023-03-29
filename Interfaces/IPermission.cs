using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IPermission
    {
        Task<ResponseStatus> Create(AccPermission model);
        Task<ResponseStatus> Update(AccPermission model);
        Task<ResponseStatus> Delete(int id);
        Task<AccPermission> GetById(int id);
        Task<List<AccPermission>> GetAll();
    }
}

