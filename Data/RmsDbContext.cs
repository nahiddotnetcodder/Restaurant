using RMS.Interfaces;
using RMS.Models;

namespace RMS.Data
{
    public class RmsDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public RmsDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<AccFiscalYear> AccFiscalYear { get; set; }
        public DbSet<AccPermission> AccPermission { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<AccBankAccounts> AccBankAccounts { get; set; }
        public virtual DbSet<AccChartClass> AccChartClass { get; set; }
        public virtual DbSet<AccChartMaster> AccChartMaster { get; set; }
        public virtual DbSet<AccChartType> AccChartType { get; set; }
        public virtual DbSet<AccGlTrans> AccGlTrans { get; set; }
        public virtual DbSet<AccJournal> AccJournal { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<MenuPermissions> MenuPermissions { get; set; }

        public virtual DbSet<HRDepartment> HRDepartment { get; set; }
        public virtual DbSet<HRDesignation> HRDesignation { get; set; }
        public virtual DbSet<HREmpAtt> HREmpAtt { get; set; }
        public virtual DbSet<HREmpDetails> HREmpDetails { get; set; } 
        public virtual DbSet<HREmpRoaster> HREmpRoaster { get; set; }
        public virtual DbSet<HRHolidays> HRHolidays { get; set; }
        public virtual DbSet<HRLeaveDetail> HRLeaveDetail { get; set; }
        public virtual DbSet<HRLeavePolicy> HRLeavePolicy { get; set; }
        public virtual DbSet<HREmpSalary> HRSalary { get; set; }
        public virtual DbSet<HRSalaryPolicy> HRSalaryPolicy { get; set; }
        public virtual DbSet<HRWeekend> HRWeekend { get; set; }
        public virtual DbSet<HRWStatus> HRWStatus { get; set; }
        public virtual DbSet<StoreDClose> StoreDClose { get; set; }
        public virtual DbSet<StoreCategory> StoreCategory { get; set; }
        public virtual DbSet<StoreGIssueMaster> StoreGIssueMasters { get; set; }
        public virtual DbSet<StoreGIssueDetails> StoreGIssueDetails { get; set; }
        public virtual DbSet<StoreGoodsStock> StoreGoodsStock { get; set; }
        public virtual DbSet<StoreGReceiveMaster> StoreGReceiveMasters { get; set; }
        public virtual DbSet<StoreGReceiveDetails> StoreGReceiveDetails { get; set; }
        public virtual DbSet<StoreIGen> StoreIGen { get; set; }
        public virtual DbSet<StoreSCategory> StoreSCategory { get; set; }
        public virtual DbSet<StoreSuppliers> StoreSupplier { get; set; }
        public virtual DbSet<StoreUnit> StoreUnit { get; set; }

        public virtual DbSet<ResFoodType> ResFoodType { get; set; }
        public virtual DbSet<ResKitchenInfo> ResKitchenInfo { get; set; }
        public virtual DbSet<ResTable> ResTable { get; set; }
        public virtual DbSet<ResMenu> ResMenu { get; set; }
        public virtual DbSet<RMMaster> RMMaster { get; set; }
        public virtual DbSet<RMDetails> RMDetails { get; set; }

    }
}
