using SmartStock.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace SmartStock.Shared.Entities;

public class Category
{
    public int CategoryId { get; set; }

    [Display(Name = "category", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "max_length", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "required_field", ErrorMessageResourceType = typeof(Literals))]
    public string CategoryName { get; set; } = null!;

    public ICollection<Product>? Products { get; set; }

    public int ProductsCount => Products == null ? 0 : Products.Count;
}