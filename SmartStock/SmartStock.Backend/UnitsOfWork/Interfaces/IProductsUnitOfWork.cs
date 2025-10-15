using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entities;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork.Interfaces;

public interface IProductsUnitOfWork
{
    Task<IEnumerable<Product>> GetComboAsync(int categoryId);

    Task<ActionResponse<Product>> AddAsync(ProductDTO teamDTO);

    Task<ActionResponse<Product>> UpdateAsync(ProductDTO teamDTO);

    Task<ActionResponse<Product>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Product>>> GetAsync();

    /// <summary>
    /// Obtiene de manera paginada la colección de productos según los parámetros especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);

    /// <summary>
    ///  Obtiene el número total de registros de productos aplicando los criterios de paginación especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}