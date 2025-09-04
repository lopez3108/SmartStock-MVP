using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Helpers;
using SmartStock.Shared.Entites;
using System.Globalization;
using System.Text;

namespace SmartStock.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public SeedDb(DataContext context, IFileStorage fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
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
            var imageExtensions = new[] { ".png", ".jpg", ".jpeg" };

            foreach (var category in _context.Categories)
            {
                // Definir producto base
                var baseProduct = new Product
                {
                    ProductCode = $"{category.CategoryName.Substring(0, Math.Min(4, category.CategoryName.Length)).ToUpper()}-001",
                    ProductName = $"{category.CategoryName} básico",
                    UnitPrice = random.Next(100, 2000),
                    CurrentStock = random.Next(10, 100),
                    MinimumStock = 5,
                    ExpirationDate = null,
                    CreatedAt = DateTime.UtcNow,
                    Category = category!,
                    CategoryId = category.CategoryId
                };

                // Buscar imagen con varias extensiones
                string? filePath = null;
                foreach (var ext in imageExtensions)
                {
                    var tempPath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{baseProduct.ProductName}{ext}";
                    if (File.Exists(tempPath))
                    {
                        filePath = tempPath;
                        break;
                    }
                }

                if (filePath != null)
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    baseProduct.Image = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                }

                _context.Products.Add(baseProduct);

                // Si la categoría es Televisores, crear más productos
                if (category.CategoryName == "Televisores")
                {
                    for (int i = 2; i <= 5; i++)
                    {
                        var tvProduct = new Product
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
                        };

                        // Buscar imagen del televisor con varias extensiones
                        string? tvFilePath = null;
                        foreach (var ext in imageExtensions)
                        {
                            var tempPath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{tvProduct.ProductName}{ext}";
                            if (File.Exists(tempPath))
                            {
                                tvFilePath = tempPath;
                                break;
                            }
                        }

                        if (tvFilePath != null)
                        {
                            var fileBytes = File.ReadAllBytes(tvFilePath);
                            tvProduct.Image = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                        }

                        _context.Products.Add(tvProduct);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    private string NormalizeFileName(string name)
    {
        // Quita acentos y caracteres raros
        var normalized = name.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        // Reemplazar espacios por guiones bajos
        return sb.ToString().Normalize(NormalizationForm.FormC)
                 .Replace(" ", "_")
                 .Replace("-", "_");
    }
}