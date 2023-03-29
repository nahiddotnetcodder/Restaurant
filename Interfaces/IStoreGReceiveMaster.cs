using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreGReceiveMaster
    {
        Task<ResponseStatus> Create(StoreGReceiveMaster model);
        Task<ResponseStatus> Update(StoreGReceiveMaster model);
        Task<List<StoreGReceiveMaster>> GetAll();
        Task<StoreGReceiveMaster> GetById(int id);
        Task<ResponseStatus> DeleteItemById(int id);
        Task<ResponseStatus> Delete(int id);
        public DateTime getdate(); //get StoreDClose Date
    }
}

