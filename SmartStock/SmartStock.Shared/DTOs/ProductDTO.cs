using SmartStock.Shared.Entites;
using SmartStock.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace SmartStock.Shared.DTOs;

public class ProductDTO
{
    public int ProductId { get; set; }

    [Display(Name = "product_code", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
    public string ProductCode { get; set; } = null!;

    [Display(Name = "product_name", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
    public string ProductName { get; set; } = null!;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Required]
    public int CurrentStock { get; set; }

    [Required]
    public int MinimumStock { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "image", ResourceType = typeof(Literals))]
    public string? Image { get; set; }

    [Display(Name = "category", ResourceType = typeof(Literals))]
    public int CategoryId { get; set; }
}