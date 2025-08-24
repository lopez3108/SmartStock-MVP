using Microsoft.EntityFrameworkCore;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCategoriesAsync();
        await CheckProductsAsync();
    }

    private async Task CheckCategoriesAsync()
    {
        if (!_context.Categories.Any())
        {
            var categoriesSQLScript = File.ReadAllText("Data\\Categories.sql");
            await _context.Database.ExecuteSqlRawAsync(categoriesSQLScript);
        }
    }

    private async Task CheckProductsAsync()
    {
        if (!_context.Products.Any())
        {
            var random = new Random();

            foreach (var category in _context.Categories)
            {
                // Crear un producto base por cada categoría
                _context.Products.Add(new Product
                {
                    ProductCode = $"{category.CategoryName.Substring(0, Math.Min(4, category.CategoryName.Length)).ToUpper()}-001",
                    ProductName = $"{category.CategoryName} básico",
                    UnitPrice = random.Next(100, 2000), // precio aleatorio de ejemplo
                    CurrentStock = random.Next(10, 100),
                    MinimumStock = 5,
                    ExpirationDate = null, // si aplica para alimentos, se puede ajustar
                    CreatedAt = DateTime.UtcNow,
                    Category = category!,
                    CategoryId = category.CategoryId
                });

                if (category.CategoryName == "Televisores")
                {
                    // Crear 4 productos adicionales en la categoría Televisores
                    for (int i = 2; i <= 5; i++)
                    {
                        _context.Products.Add(new Product
                        {
                            ProductCode = $"TELE-{i:D3}",
                            ProductName = $"Televisor modelo {i}",
                            UnitPrice = random.Next(1000, 5000),
                            CurrentStock = random.Next(5, 50),
                            MinimumStock = 3,
                            ExpirationDate = null,
                            CreatedAt = DateTime.UtcNow,
                            Category = category!,
                            CategoryId = category.CategoryId
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}