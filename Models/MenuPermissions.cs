namespace RMS.Models
{
    public class MenuPermissions
    {
        [Key]
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int? MenuId { get; set; }
        public int? MenuItemId { get; set; }
        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string CUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
