
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IHRLeaveDetails
    {
        PaginatedList<HRLeaveDetail> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10); //read all
        HRLeaveDetail GetItem(int id); // read particular item
        HRLeaveDetail Create(HRLeaveDetail hrleavedetails);
        HRLeaveDetail Edit(HRLeaveDetail hrleavedetails);
        HRLeaveDetail Delete(HRLeaveDetail hrleavedetails);
        public bool IsNameExists(int Did);
        public bool IsNameExists( DateTime indate, DateTime outdate);

    }
}

