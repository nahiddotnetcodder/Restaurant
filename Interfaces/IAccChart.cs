using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IAccChart
    {
        Task<ResponseStatus> Create(AccChartClass model);
        Task<ResponseStatus> Update(AccChartClass model);
        Task<ResponseStatus> Delete(int id);
        Task<AccChartClass> GetById(int id);
        Task<List<AccChartClass>> GetAll();
    }
}

