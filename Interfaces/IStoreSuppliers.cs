using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IStoreSuppliers
    {
        PaginatedList<StoreSuppliers> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        StoreSuppliers GetItem(int id); // read particular item
        StoreSuppliers Create(StoreSuppliers model);
        StoreSuppliers Edit(StoreSuppliers model);
        StoreSuppliers Delete(StoreSuppliers model);
        public bool IsEmpCodeExists(int Id);
        public bool IsEmpCodeExists(string name,int Id );
        Task<List<StoreSuppliers>> GetAll();
    }
} 

