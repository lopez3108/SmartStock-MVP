namespace SmartStock.Shared.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    // Opcional: si quieres enviar el conteo al frontend
    public int ProductsCount { get; set; }
}