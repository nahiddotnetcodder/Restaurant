using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IRecManagerMaster
    {
        Task<ResponseStatus> Create(RMMaster model);
        Task<ResponseStatus> Update(RMMaster model);
        Task<List<RMMaster>> GetAll();
        Task<RMMaster> GetById(int id);
        Task<ResponseStatus> DeleteItemById(int id);
        Task<ResponseStatus> Delete(int id);
    }
}

