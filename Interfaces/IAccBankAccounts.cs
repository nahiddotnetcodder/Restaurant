using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IAccBankAccounts
    {
        Task<ResponseStatus> Create(AccBankAccounts model);
        Task<ResponseStatus> Update(AccBankAccounts model);
        Task<List<AccBankAccounts>> GetAll();
        Task<AccBankAccounts> GetById(int id);
        Task<ResponseStatus> DeleteItemById(int id);
    }
}

