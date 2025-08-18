using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [MaxLength(100)]
    public string? Category { get; set; }

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
}