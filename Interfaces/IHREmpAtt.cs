
namespace RMS.Interfaces
{
    public interface IHREmpAtt
    {
        PaginatedList<HREmpAtt> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10); //read all
        HREmpAtt GetItem(int id); // read particular item
        HREmpAtt Create(HREmpAtt hrempatt);
        HREmpAtt Edit(HREmpAtt hrempatt);
        HREmpAtt Delete(HREmpAtt hrempatt);
        public bool IsENameExists(string name);
        public bool IsENameExists(int id, DateTime date);
    }
}

