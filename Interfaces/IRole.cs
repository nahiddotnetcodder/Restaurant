using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IRole
    {
        Task<ResponseStatus> CreateRole(ApplicationRole model, List<MenuItem> menuItems);
        Task<ResponseStatus> UpdateRole(ApplicationRole model, List<MenuItem> menuItems);
        Task<ApplicationRole> GetRoleById(string id);
        Task<List<ApplicationRole>> GetAll();
        Task<List<TextValuePair>> GetAllAsTextValuePair();
        Task<List<MenuPermissions>> GetAllMenuPermissions(string roleId);
        Task<List<MenuPermissions>> GetUserAssignedMenus();
    }
}

