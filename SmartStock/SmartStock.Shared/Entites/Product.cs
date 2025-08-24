using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace SmartStock.Shared.Entites;

public class Product
{
    public int ProductId { get; set; }

    [MaxLength(100)]
    [Required]
    public string ProductCode { get; set; } = null!;

    [MaxLength(100)]
    [Required]
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

    public Category Category { get; set; } = null!;

    public int CategoryId { get; set; }
}