using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.Helpers;
using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entities;
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
        var category = await _context.Categories.FindAsync(productDTO.CategoryId);
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
            ProductCode = productDTO.ProductCode,
            ProductName = productDTO.ProductName,
            UnitPrice = productDTO.UnitPrice,
            CurrentStock = productDTO.CurrentStock,
            MinimumStock = productDTO.MinimumStock,
            ExpirationDate = productDTO.ExpirationDate,
            CreatedAt = productDTO.CreatedAt,
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
        // 1️⃣ Buscar el producto existente por su ID
        var currentProduct = await _context.Products.FindAsync(productDTO.ProductId);
        if (currentProduct == null)
        {
            // Retorna error si no existe el producto
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR005" // Producto no encontrado
            };
        }

        // 2️⃣ Buscar la categoría asociada al producto
        var category = await _context.Categories.FindAsync(productDTO.CategoryId);
        if (category == null)
        {
            // Retorna error si no existe la categoría
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR004" // Categoría no encontrada
            };
        }

        // 3️⃣ Guardado de la imagen (esta es la parte que da problemas con Azure)
        if (!string.IsNullOrEmpty(productDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(productDTO.Image!);
            // ⚠️ Aquí se intenta subir la imagen a Azure Blob Storage
            currentProduct.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "products");
        }

        // 4️⃣ Actualizar campos del producto
        currentProduct.Category = category;
        currentProduct.ProductCode = productDTO.ProductCode;
        currentProduct.ProductName = productDTO.ProductName;
        currentProduct.UnitPrice = productDTO.UnitPrice;
        currentProduct.CurrentStock = productDTO.CurrentStock;
        currentProduct.MinimumStock = productDTO.MinimumStock;
        currentProduct.ExpirationDate = productDTO.ExpirationDate;
        currentProduct.CreatedAt = productDTO.CreatedAt;

        // 5️⃣ Marcar el producto como modificado en el DbContext
        _context.Update(currentProduct);

        try
        {
            // 6️⃣ Guardar cambios en la base de datos
            await _context.SaveChangesAsync();
            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = currentProduct
            };
        }
        catch (DbUpdateException)
        {
            // Manejo de error específico de EF
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            // Manejo de cualquier otro error
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    /// <summary>
    /// Obtiene de manera paginada la colección de productos según los parámetros especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Products
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.ProductName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Product>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.ProductName)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    /// <summary>
    ///  Obtiene el número total de registros de productos aplicando los criterios de paginación especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.ProductName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}