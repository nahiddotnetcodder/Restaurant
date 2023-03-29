using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreGIssueMaster
    {
        Task<ResponseStatus> Create(StoreGIssueMaster model);
        Task<ResponseStatus> Update(StoreGIssueMaster model);
        Task<List<StoreGIssueMaster>> GetAll();
        Task<StoreGIssueMaster> GetById(int id);
        Task<ResponseStatus> DeleteItemById(int id);
        Task<ResponseStatus> Delete(int id);
        public DateTime getdate(); //get StoreDClose Date
    }
}

