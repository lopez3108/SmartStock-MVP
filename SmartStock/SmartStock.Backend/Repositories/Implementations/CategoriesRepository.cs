using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.Repositories.Implementations;

public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
{
    private readonly DataContext _context;

    public CategoriesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las categorías registradas en la base de datos,
    /// incluyendo sus productos asociados.
    /// </summary>
    /// <returns>
    /// Retorna un <see cref="ActionResponse{IEnumerable{Category}}"/>
    /// con la lista completa de categorías.
    /// </returns>
    public override async Task<ActionResponse<IEnumerable<Category>>> GetAsync()
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Category>>
        {
            WasSuccess = true,
            Result = categories
        };
    }

    /// <summary>
    /// Obtiene una categoría específica por su identificador,
    /// incluyendo sus productos asociados.
    /// </summary>
    /// <param name="id">Identificador único de la categoría.</param>
    /// <returns>
    /// Retorna un <see cref="ActionResponse{Category}"/> con la categoría encontrada.
    /// Si no existe, retorna un error con el código "ERR001".
    /// </returns>
    public override async Task<ActionResponse<Category>> GetAsync(int id)
    {
        var category = await _context.Categories
             .Include(c => c.Products)
             .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Category>
        {
            WasSuccess = true,
            Result = category
        };
    }

    /// <summary>
    /// Obtiene una lista simplificada de categorías,
    /// ordenadas alfabéticamente por nombre.
    /// </summary>
    /// <returns>
    /// Retorna una colección enumerable de categorías,
    /// pensada para poblar combos o listas desplegables.
    /// </returns>
    public async Task<IEnumerable<Category>> GetComboAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }
}