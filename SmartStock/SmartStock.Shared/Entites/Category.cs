using System.ComponentModel.DataAnnotations;

namespace SmartStock.Shared.Entites;

public class Category
{
    public int CategoryId { get; set; }

    [MaxLength(100)]
    [Required]
    public string CategoryName { get; set; } = null!;

    public ICollection<Product>? Products { get; set; }

    public int ProductsCount => Products == null ? 0 : Products.Count;
}