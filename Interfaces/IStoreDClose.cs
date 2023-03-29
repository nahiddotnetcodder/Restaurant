
namespace RMS.Interfaces
{
    public interface IStoreDClose
    {
        PaginatedList<StoreDClose> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        StoreDClose GetItem(int rdcid); // read particular item
        StoreDClose Create(StoreDClose storedclose);
        StoreDClose Edit(StoreDClose storedclose);

        public DateTime getDate(); 
    }
}

