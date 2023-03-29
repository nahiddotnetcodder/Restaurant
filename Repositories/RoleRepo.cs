using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace RMS.Repositories
{
    public class RoleRepo : IRole
    {
        private readonly RmsDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleRepo(RmsDbContext context, RoleManager<ApplicationRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseStatus> CreateRole(ApplicationRole model, List<MenuItem> menuItems)
        {
            var status = new ResponseStatus();
            var currentUser = GetCurrentUser();
            var isExistsRole = await _context.ApplicationRoles.Where(x=> x.Name == model.Name).FirstOrDefaultAsync();
            if(isExistsRole != null)
            {
                status.StatusCode = 0;
                status.Message = "Role Already Exists";
                return status;
            }
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                var roleModel = new IdentityRole();
                model.Id = roleModel.Id;

                var isSuccess = false;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        _context.ApplicationRoles.Add(model);
                        var result = await _context.SaveChangesAsync();

                        var allPermissions = await _context.MenuPermissions.ToListAsync();
                        var menuPermissions = new MenuPermissions();
                        foreach (var item in menuItems)
                        {
                            var existingPermission = allPermissions.Where(x => x.RoleId == model.Id && x.MenuId == item.MenuId && x.MenuItemId == item.Id).FirstOrDefault();
                            if (existingPermission == null)
                            {
                                menuPermissions = new MenuPermissions
                                {
                                    RoleId = model.Id,
                                    MenuId = item.MenuId,
                                    MenuItemId = item.Id,
                                    IsActive = item.IsSelected,
                                    CUser = currentUser.FullName
                                };
                                _context.MenuPermissions.Add(menuPermissions);
                            }
                            else
                            {
                                existingPermission.IsActive = item.IsSelected;
                                _context.Update(existingPermission);
                            }
                            await _context.SaveChangesAsync();
                        }
                        scope.Complete();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        scope.Dispose();
                    }
                }

                if (!isSuccess)
                {
                    status.StatusCode = 0;
                    status.Message = "Role Creation Failed";
                    return status;
                }
            }
            status.StatusCode = 1;
            status.Message = "Role Created successfully";
            return status;
        }

        public async Task<ApplicationRole> GetRoleById(string id)
        {
            var role = await _context.ApplicationRoles.Where(x=> x.Id == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<ResponseStatus> UpdateRole(ApplicationRole model, List<MenuItem> menuItems)
        {
            var currentUser = GetCurrentUser();
            var status = new ResponseStatus();
            var role = await _context.ApplicationRoles.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            role.Name = model.Name;
            role.Description = model.Description;
            role.CurrentStatusId = model.CurrentStatusId;

            var isSuccess = false;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await _roleManager.UpdateAsync(role);
                    var allPermissions = await _context.MenuPermissions.ToListAsync();
                    var menuPermissions = new MenuPermissions();
                    foreach(var item in menuItems)
                    {
                        var existingPermission = allPermissions.Where(x => x.RoleId == role.Id && x.MenuId == item.MenuId && x.MenuItemId == item.Id).FirstOrDefault();
                        if(existingPermission == null)
                        {
                            menuPermissions = new MenuPermissions
                            {
                                RoleId = role.Id,
                                MenuId = item.MenuId,
                                MenuItemId = item.Id,
                                IsActive = item.IsSelected,
                                CUser = currentUser.FullName
                            };
                            _context.MenuPermissions.Add(menuPermissions);
                        }
                        else
                        {
                            existingPermission.IsActive = item.IsSelected;
                            _context.Update(existingPermission);
                        }
                        await _context.SaveChangesAsync();
                    }
                    scope.Complete();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    scope.Dispose();
                }
            }

            if (!isSuccess)
            {
                status.StatusCode = 0;
                status.Message = "Role Update Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "Role Updated successfully";
            return status;
        }
        public async Task<List<ApplicationRole>> GetAll()
        {
            var roles = await _context.ApplicationRoles.ToListAsync();
            return roles;
        }

        public async Task<List<TextValuePair>> GetAllAsTextValuePair()
        {
            var roles = await _context.ApplicationRoles.Where(x=> x.IsActive).ToListAsync();
            return roles.Select(v => new TextValuePair
            {
                Id = v.Id,
                Name = v.Name,
            }).ToList();
        }

        public async Task<List<MenuPermissions>> GetAllMenuPermissions(string roleId)
        {
            var data = await _context.MenuPermissions.Where(x=> x.RoleId == roleId).Include(x=> x.ApplicationRole).Include(x=> x.Menu).Include(x => x.MenuItem).ToListAsync();
            return data;
        }
        public ApplicationUser GetCurrentUser()
        {
            var currentUserString = _httpContextAccessor.HttpContext.Session.GetString(ApplicationConstants.SessionEntity);
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(currentUserString);
            return currentUser;
        }
        public string GetCurrentUserRole()
        {
            var currentUser = GetCurrentUser();
            var currentUserRole = _context.UserRoles.FirstOrDefault(x => x.UserId == currentUser.Id);
            return currentUserRole.RoleId;
        }
        public async Task<List<MenuPermissions>> GetUserAssignedMenus()
        {
            var roleId = GetCurrentUserRole();
            var data = await _context.MenuPermissions.Where(x => x.RoleId == roleId).Include(x => x.ApplicationRole).Include(x => x.Menu).Include(x => x.MenuItem).ToListAsync();
            return data;
        }
    }
}
