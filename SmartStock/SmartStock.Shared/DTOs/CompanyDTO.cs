using SmartStock.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace SmartStock.Shared.DTOs;

public class CompanyDTO
{
    public int CompanyId { get; set; }

    [Display(Name = "company_name", ResourceType = typeof(Literals))]
    [MaxLength(150, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
    public string CompanyName { get; set; } = null!;

    [Display(Name = "company_email", ResourceType = typeof(Literals))]
    [MaxLength(150, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    [EmailAddress(ErrorMessageResourceName = "invalid_email", ErrorMessageResourceType = typeof(Literals))]
    public string? CompanyEmail { get; set; }

    [Display(Name = "company_phone", ResourceType = typeof(Literals))]
    [MaxLength(50, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    public string? CompanyPhone { get; set; }

    [Display(Name = "company_address", ResourceType = typeof(Literals))]
    [MaxLength(200, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    public string? CompanyAddress { get; set; }

    [Display(Name = "company_nit", ResourceType = typeof(Literals))]
    [MaxLength(50, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    public string? CompanyNIT { get; set; }

    [Display(Name = "is_active", ResourceType = typeof(Literals))]
    public bool IsActive { get; set; } = true;

    [Display(Name = "created_at", ResourceType = typeof(Literals))]
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Display(Name = "created_by", ResourceType = typeof(Literals))]
    public int? CreatedBy { get; set; }

    [Display(Name = "modified_at", ResourceType = typeof(Literals))]
    public DateTime? ModifiedAt { get; set; }

    [Display(Name = "modified_by", ResourceType = typeof(Literals))]
    public int? ModifiedBy { get; set; }
}