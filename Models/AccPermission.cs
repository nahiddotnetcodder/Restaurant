using System.ComponentModel;

namespace RMS.Models
{
    public class AccPermission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Permission Key")]
        public string Key { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Menu Name")]
        public string MenuName { get; set; }
        [Required]
        [DisplayName("Menu Item")]
        public string MenuItem { get; set; }
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}