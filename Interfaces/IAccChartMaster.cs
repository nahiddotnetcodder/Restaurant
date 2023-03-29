using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IAccChartMaster
    {
        Task<ResponseStatus> Create(AccChartMaster model);
        Task<ResponseStatus> Update(AccChartMaster model);
        Task<ResponseStatus> Delete(int id);
        Task<AccChartMaster> GetById(int id);
        Task<List<AccChartMaster>> GetAllIncludeInactive();
        Task<List<AccChartMaster>> GetAll();
      
    }
}

