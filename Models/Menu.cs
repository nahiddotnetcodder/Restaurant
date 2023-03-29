using static RMS.Models.ApplicationConstants;

namespace RMS.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string StatusName
        {
            get
            {
                if (StatusId <= 0)
                    return string.Empty;
                return EnumUtility.GetDescriptionFromEnumValue((Status)StatusId);
            }
        }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}