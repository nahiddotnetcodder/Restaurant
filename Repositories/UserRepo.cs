
using System.Threading.Tasks;

namespace RMS.Repositories
{
    public class UserRepo : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly RmsDbContext _context;
        public UserRepo(RmsDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ResponseStatus> CreateUser(User model)
        {
            var status = new ResponseStatus();
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User Already Exists";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                UserLogin = model.UserLogin,
                FullName = model.FullName,
                //For login check FindByNameAsync, so set property here for username = userlogin
                UserName = model.UserLogin,
                Email = model.Email,
                IsActive = model.IsActive,
                AccessLevelId = model.AccessLevelId,
                SecurityStamp = Guid.NewGuid().ToString(),
                NormalizedUserName = model.FullName,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Creation Failed";
                return status;
            }

            //assign to role
            var role = await _roleManager.FindByIdAsync(model.AccessLevelId);
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }
        public async Task<List<UserViewModel>> GetUsers()
        {

            var data = from user in _context.ApplicationUsers.Where(x => x.IsActive)
                       join role in _context.ApplicationRoles on user.AccessLevelId equals role.Id
                       select new UserViewModel
                       {
                           Id = user.Id,
                           UserLogin = user.UserLogin,
                           FullName = user.FullName,
                           Email = user.Email,
                           RoleName = role.Name
                       };

            return await data.OrderByDescending(x => x.Id).ToListAsync();
        }
        public async Task<UserViewModel> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Password = string.Empty,
                UserLogin = user.UserLogin,
                FullName = user.FullName,
                Email = user.Email,
                AccessLevelId = user.AccessLevelId,
                IsActive = user.IsActive
            };
            return userViewModel;
        }

        public async Task<ResponseStatus> UpdateUser(UserViewModel model)
        {
            var status = new ResponseStatus();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "No User Found";
                return status;
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.AccessLevelId = model.AccessLevelId;
            user.IsActive = model.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var newPassword = await _userManager.ResetPasswordAsync(user, token, model.Password);
            }
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Creation Failed";
                return status;
            }

            //remove user from role
            var userRoles = await _userManager.GetRolesAsync(user);
            var removeRole = await _userManager.RemoveFromRolesAsync(user, userRoles);
            //assign to role
            var role = await _roleManager.FindByIdAsync(model.AccessLevelId);
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }
    }
}
