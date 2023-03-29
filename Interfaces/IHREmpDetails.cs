

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHREmpDetails
    {
        PaginatedList<HREmpDetails> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        HREmpDetails GetItem(int edid); // read particular item
        HREmpDetails Create(HREmpDetails empdetails);
        HREmpDetails Edit(HREmpDetails empdetails);
        HREmpDetails Delete(HREmpDetails empdetails);
        public bool IsEmpCodeExists(string empcode);
        //public bool IsEmpCodeExists(string empcode, string empname);
        public bool IsEmpCodeExists(string name, int id);
        public string GetEmpID(); ///auto-Genarate EmpID 
        Task<List<HREmpDetails>> GetAll();
        
    }
} 

