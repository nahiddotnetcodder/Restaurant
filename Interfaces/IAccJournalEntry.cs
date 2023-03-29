using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IAccJournalEntry
    {
        Task<string> GetRefPrefixNo();
        Task<ResponseStatus> Create(AccJournal model);
        Task<ResponseStatus> Update(AccJournal model);
        Task<List<AccJournalViewModel>> GetAll();
        Task<AccJournalViewModel> GetById(int id);
        Task<ResponseStatus> DeleteItemById(int id);
    }
}

