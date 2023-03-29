
namespace RMS.Interfaces
{
    public interface IHREmpSalary
    {
        PaginatedList<HREmpSalary> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10); //read all
        HREmpSalary GetItem(int id); // read particular item
        HREmpSalary Create(HREmpSalary hrsalary);
        HREmpSalary Edit(HREmpSalary hrsalary);
        HREmpSalary Delete(HREmpSalary hrsalary);
        public bool IsDNameExists(int id);
        public bool IsDNameExists(int name, int Id);
    }
}

