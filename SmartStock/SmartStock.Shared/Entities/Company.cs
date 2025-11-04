using global::SmartStock.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace SmartStock.Shared.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [Display(Name = "company_name", ResourceType = typeof(Literals))]
        [MaxLength(150, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
        public string Name { get; set; } = null!;

        [Display(Name = "company_nit", ResourceType = typeof(Literals))]
        [MaxLength(50, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
        public string NIT { get; set; } = null!;

        [Display(Name = "company_address", ResourceType = typeof(Literals))]
        [MaxLength(200, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        public string? Address { get; set; }

        [Display(Name = "company_phone", ResourceType = typeof(Literals))]
        [MaxLength(50, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        public string? Phone { get; set; }

        [Display(Name = "company_email", ResourceType = typeof(Literals))]
        [MaxLength(100, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        [EmailAddress(ErrorMessageResourceName = "invalid_email", ErrorMessageResourceType = typeof(Literals))]
        public string? Email { get; set; }

        [Display(Name = "is_active", ResourceType = typeof(Literals))]
        public bool IsActive { get; set; } = true;

        [Display(Name = "created_by", ResourceType = typeof(Literals))]
        [MaxLength(100, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        public string? CreatedBy { get; set; }

        [Display(Name = "created_at", ResourceType = typeof(Literals))]
        [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "modified_by", ResourceType = typeof(Literals))]
        [MaxLength(100, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
        public string? ModifiedBy { get; set; }

        [Display(Name = "modified_at", ResourceType = typeof(Literals))]
        public DateTime? ModifiedAt { get; set; }
    }
}