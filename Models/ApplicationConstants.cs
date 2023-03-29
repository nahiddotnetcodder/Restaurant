using System.ComponentModel;

namespace RMS.Models
{
    public class ApplicationConstants
    {
        public const string SessionEntity = "SessionEntity";
        public static class PermissionMenus
        {
            public const string Admin_SetUp = "Admin_SetUp";
            public const string Acc_SetUp = "Acc_SetUp";
        }
        public class MenuItemsConstant
        {
            public const string Users = "Users";
            public const string Roles = "Roles";
            public const string Menus = "Menus";
            public const string Menu_Items = "Menu Items";
            public const string Permissions = "Permissions";
            public const string Chart_Class = "Chart Class";
            public const string Chart_Type = "Chart Type";
            public const string Chart_Master = "Chart Master";
            public const string Fiscal_Year = "Fiscal Year";
            public const string Journal_Entry = "Journal Entry";
            public const string Journal_Lists = "Journal Lists";
            public const string Restaurant_Setup = "Restaurant Setup";
            public const string Kitchen_Setup = "Kitchen Setup";
            public const string Food_Type_Setup = "Food Type Setup";
            public const string Food_Menu_Setup = "Food Menu Setup";
            public const string Date_Setup = "Date Setup";
            public const string Department_Setup = "Department Setup";
            public const string Designation_Setup = "Designation Setup";
            public const string Work_Status_Setup = "Work Status Setup";
            public const string Employee_Details_Setup = "Employee Details Setup";
            public const string Billing = "Billing";
            public const string Units = "Units";
            public const string Products = "Products";
            public const string Categories = "Categories";
            public const string Brands = "Brands";
            public const string Product_Profiles = "Product Profiles";
            public const string Product_Groups = "Product Groups";
            public const string Suppliers = "Suppliers";
            public const string Currencies = "Currencies";
        }
        public class ControllerConstant
        {
            public const string User = "User";
            public const string Role = "Role";
            public const string Menu = "Menu";
            public const string MenuItem = "MenuItem";
            public const string Permission = "Permission";
            public const string AccChart = "AccChart";
            public const string AccChartType = "AccChartType";
            public const string AccChartMaster = "AccChartMaster";
            public const string AccFiscalYear = "AccFiscalYear";
            public const string AccJournalEntry = "AccJournalEntry";
            public const string ResInfo = "ResInfo";
            public const string ResKitchenInfo = "ResKitchenInfo";
            public const string ResFoodType = "ResFoodType";
            public const string ResMenu = "ResMenu";
            public const string ResDClose = "ResDClose";
            public const string HRDepartment = "HRDepartment";
            public const string HRDesignation = "HRDesignation";
            public const string HRWStatus = "HRWStatus";
            public const string HREmpDetails = "HREmpDetails";
            public const string ResSales = "ResSales";
            public const string Unit = "Unit";
            public const string Product = "Product";
            public const string Category = "Category";
            public const string Brand = "Brand";
            public const string ProductProfile = "ProductProfile";
            public const string ProductGroup = "ProductGroup";
            public const string Supplier = "Supplier";
            public const string Currency = "Currency";
        }
        public enum UserRole
        {
            [Description("Admin")]
            Admin = 1,
            [Description("HoD")]
            HoD = 2,
            [Description("User")]
            User = 3
        }
        public enum Status
        {
            [Description("Active")]
            Active = 1,
            [Description("InActive")]
            InActive = 2
        }
    }
}
