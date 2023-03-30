
namespace RMS.Interfaces
{
    public interface IResInfo
    {
        PaginatedList<ResInfo> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 1); //read all
        ResInfo GetItem(int id); // read particular item

        ResInfo Create(ResInfo resinfo);

        ResInfo Edit(ResInfo resinfo);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int Id);


    }
}

