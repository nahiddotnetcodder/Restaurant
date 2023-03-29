using RMS.Models;
using System.Threading.Tasks;

namespace RMS.Interfaces
{
    public interface IUser
    {
        Task<ResponseStatus> CreateUser(User model);
        Task<ResponseStatus> UpdateUser(UserViewModel model);
        Task<UserViewModel> GetUserById(string id);
        Task<List<UserViewModel>> GetUsers();
    }
}

