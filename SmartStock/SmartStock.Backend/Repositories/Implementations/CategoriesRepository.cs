using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.Helpers;
using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Shared.DTOs;
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
            .OrderBy(c => c.CategoryName)
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

    /// <summary>
    /// Obtiene de forma paginada el listado de categorías según los parámetros de paginación recibidos.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public override async Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Categories
            .Include(x => x.Products)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.CategoryName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Category>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.CategoryName)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    /// <summary>
    /// Obtiene el número total de registros disponibles en la base de datos.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.CategoryName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}