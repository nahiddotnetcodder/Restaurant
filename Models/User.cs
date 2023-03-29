namespace RMS.Models
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "User Login")]
        public string UserLogin { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string FullName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The password must be 6 digit long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Access Level")]
        public string AccessLevelId { get; set; }
        public bool IsActive { get; set; }
    }
}
