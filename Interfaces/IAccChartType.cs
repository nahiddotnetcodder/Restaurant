using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IAccChartType
    {
        Task<ResponseStatus> Create(AccChartType model);
        Task<ResponseStatus> Update(AccChartType model);
        Task<ResponseStatus> Delete(int id);
        Task<AccChartType> GetById(int id);
        Task<List<AccChartType>> GetAll();
    }
}

