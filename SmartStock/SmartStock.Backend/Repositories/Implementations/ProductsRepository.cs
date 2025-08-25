using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.Helpers;
using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.Repositories.Implementations;

public class ProductsRepository : GenericRepository<Product>, IProductsRepository
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public ProductsRepository(DataContext context, IFileStorage fileStorage) : base(context)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync()
    {
        var teams = await _context.Products
            .Include(x => x.Category)
            .OrderBy(x => x.ProductName)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Product>>
        {
            WasSuccess = true,
            Result = teams
        };
    }

    public override async Task<ActionResponse<Product>> GetAsync(int id)
    {
        var product = await _context.Products
             .Include(x => x.Category)
             .FirstOrDefaultAsync(c => c.ProductId == id);

        if (product == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Product>
        {
            WasSuccess = true,
            Result = product
        };
    }

    public async Task<ActionResponse<Product>> AddAsync(ProductDTO productDTO)
    {
        var category = await _context.Categories.FindAsync(productDTO.ProductId);
        if (category == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        var product = new Product
        {
            Category = category,
            ProductName = productDTO.ProductName,
        };

        if (!string.IsNullOrEmpty(productDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(productDTO.Image!);
            product.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "products");
        }

        _context.Add(product);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = product
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<Product>> GetComboAsync(int categoryId)
    {
        return await _context.Products
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.ProductName)
            .ToListAsync();
    }

    public async Task<ActionResponse<Product>> UpdateAsync(ProductDTO productDTO)
    {
        var currentProduct = await _context.Products.FindAsync(productDTO.ProductId);
        if (currentProduct == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR005"
            };
        }

        var category = await _context.Categories.FindAsync(productDTO.CategoryId);
        if (category == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        if (!string.IsNullOrEmpty(productDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(productDTO.Image!);
            currentProduct.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "products");
        }

        currentProduct.Category = category;
        currentProduct.ProductCode = productDTO.ProductCode;
        currentProduct.ProductName = productDTO.ProductName;
        currentProduct.UnitPrice = productDTO.UnitPrice;
        currentProduct.CurrentStock = productDTO.CurrentStock;
        currentProduct.MinimumStock = productDTO.MinimumStock;
        currentProduct.ExpirationDate = productDTO.ExpirationDate;
        currentProduct.CreatedAt = productDTO.CreatedAt;

        _context.Update(currentProduct);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = currentProduct
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}