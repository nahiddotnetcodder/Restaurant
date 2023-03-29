using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IAccFiscalYear
    {
        Task<string> GetRefPrefixNo();
        Task<ResponseStatus> Create(AccFiscalYear model);
        Task<ResponseStatus> Update(AccFiscalYear model);
        Task<ResponseStatus> Delete(int id);
        Task<AccFiscalYear> GetById(int id);
        Task<List<AccFiscalYear>> GetAll();
    }
}

