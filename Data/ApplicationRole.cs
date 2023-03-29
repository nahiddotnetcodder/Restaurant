using static RMS.Models.ApplicationConstants;

namespace RMS.Data
{
    public class ApplicationRole : IdentityRole
    {
        [Column(TypeName = "nvarchar(250)")]
        public string Description { get; set; }
        public int CurrentStatusId { get; set; }
        public bool IsActive { get; set; } = true;
        public string CurrentStatusName
        {
            get
            {
                if (CurrentStatusId <= 0)
                    return string.Empty;
                return EnumUtility.GetDescriptionFromEnumValue((Status)CurrentStatusId);
            }
        }
        [NotMapped]
        public string SelectedMenuItems { get; set; }
    }
}
