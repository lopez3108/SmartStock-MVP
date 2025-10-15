using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entities;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.Repositories.Interfaces;

public interface IProductsRepository
{
    /// <summary>
    /// Obtiene un listado de productos pertenecientes a una categoría específica,
    /// pensado para combos o menús desplegables.
    /// </summary>
    /// <param name="categoryId">Identificador único de la categoría.</param>
    /// <returns>Una colección de productos asociados a la categoría.</returns>
    Task<IEnumerable<Product>> GetComboAsync(int categoryId);

    /// <summary>
    /// Agrega un nuevo producto al sistema.
    /// </summary>
    /// <param name="productDTO">Objeto de transferencia de datos con la información del producto.</param>
    /// <returns>
    /// Un ActionResponse que contiene el producto creado y el estado de la operación.
    /// </returns>
    Task<ActionResponse<Product>> AddAsync(ProductDTO productDTO);

    /// <summary>
    /// Actualiza la información de un producto existente.
    /// </summary>
    /// <param name="productDTO">Objeto de transferencia de datos con la información actualizada del producto.</param>
    /// <returns>
    /// Un ActionResponse que contiene el producto actualizado y el estado de la operación.
    /// </returns>
    Task<ActionResponse<Product>> UpdateAsync(ProductDTO productDTO);

    /// <summary>
    /// Obtiene un producto específico según su identificador.
    /// </summary>
    /// <param name="id">Identificador único del producto.</param>
    /// <returns>
    /// Un ActionResponse que contiene el producto solicitado y el estado de la operación.
    /// </returns>
    Task<ActionResponse<Product>> GetAsync(int id);

    /// <summary>
    /// Obtiene todos los productos registrados en el sistema.
    /// </summary>
    /// <returns>
    /// Un ActionResponse que contiene la colección de productos y el estado de la operación.
    /// </returns>
    Task<ActionResponse<IEnumerable<Product>>> GetAsync();

    /// <summary>
    /// Obtiene de manera paginada la colección de productos según los parámetros especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);

    /// <summary>
    /// Obtiene el número total de registros de productos aplicando los criterios de paginación especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}