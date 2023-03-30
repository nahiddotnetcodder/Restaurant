
namespace RMS.Repositories
{
    public class ResInfoRepo : IResInfo
    {
        private readonly RmsDbContext _context; // for connecting to efcore.
        public ResInfoRepo(RmsDbContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public ResInfo Create(ResInfo resinfo)
        {
            _context.ResInfo.Add(resinfo);
            _context.SaveChanges();
            return resinfo;
        }
        
        public ResInfo Edit(ResInfo resinfo)
        {
            _context.ResInfo.Attach(resinfo);
            _context.Entry(resinfo).State = EntityState.Modified;
            _context.SaveChanges();
            return resinfo;
        }

        private List<ResInfo> DoSort(List<ResInfo> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "RName")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.RName).ToList();
                else
                    items = items.OrderByDescending(n => n.RName).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.RAddress).ToList();
                else
                    items = items.OrderByDescending(d => d.RAddress).ToList();
            }
            return items;
        }

        public PaginatedList<ResInfo> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ResInfo> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.ResInfo.Where(n => n.RName.Contains(SearchText) || n.RAddress.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.ResInfo.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<ResInfo> retItems = new PaginatedList<ResInfo>(items, pageIndex, pageSize);

            return retItems;
        }

        public ResInfo GetItem(int id)
        {
            ResInfo item = _context.ResInfo.Where(u => u.ResId == id).FirstOrDefault();
            item.RCLogoName = GetBriefPhotoName(item.RCLogoUrl);
            return item;
        }
        private string GetBriefPhotoName(string fileName)
        {
            if (fileName == null)
                return "";
            string[] words = fileName.Split('_');
            return words[words.Length - 1];
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.ResInfo.Where(n => n.RName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int Id)
        {
            int ct = _context.ResInfo.Where(n => n.RName.ToLower() == name.ToLower() && n.ResId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
