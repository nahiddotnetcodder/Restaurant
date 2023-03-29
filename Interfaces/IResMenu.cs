
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IResMenu
    {
        PaginatedList<ResMenu> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        ResMenu GetItem(int id); // read particular item
        ResMenu Create(ResMenu model);
        ResMenu Edit(ResMenu model);
        ResMenu Delete(ResMenu model);
        public bool IsItemExists(string name);
        public bool IsItemExists(string code, int id);
        Task<List<ResMenu>> GetAll();
    }
}

