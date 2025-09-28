using SmartStock.Shared.DTOs;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Obtiene una entidad por su identificador único.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Un objeto con la entidad encontrada o un error si no existe.</returns>
    Task<ActionResponse<T>> GetAsync(int id);

    /// <summary>
    /// Obtiene todas las entidades de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <returns> Una lista de entidades.</returns>
    Task<ActionResponse<IEnumerable<T>>> GetAsync();

    /// <summary>
    /// Inserta una nueva entidad en el repositorio.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns> La entidad creada con el resultado de la operación.</returns>
    Task<ActionResponse<T>> AddAsync(T entity);

    /// <summary>
    /// Elimina una entidad del repositorio por su identificador.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Resultado de la operación con la entidad eliminada.</returns>
    Task<ActionResponse<T>> DeleteAsync(int id);

    /// <summary>
    /// Actualiza una entidad existente en el repositorio.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>La entidad actualizada con el resultado de la operación.</returns>
    Task<ActionResponse<T>> UpdateAsync(T entity);

    /// <summary>
    /// Obtiene una lista paginada de registros a partir de los parámetros de paginación enviados en la consulta.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);

    /// <summary>
    ///  Obtiene el número total de registros disponibles en la base de datos.
    /// </summary>
    /// <returns></returns>
    Task<ActionResponse<int>> GetTotalRecordsAsync();
}