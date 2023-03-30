using static RMS.Models.ApplicationConstants;

namespace RMS.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public int MenuId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        [NotMapped]
        public string MenuName { get; set; }
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
        [NotMapped]
        public bool IsSelected { get;set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
    }
}
