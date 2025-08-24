using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _entity = context.Set<T>();
    }

    /// <summary>
    /// Agrega una nueva entidad del tipo <typeparamref name="T"/> al contexto de base de datos
    /// y guarda los cambios de forma asincrónica.
    /// </summary>
    /// <param name="entity">La entidad que se desea agregar al contexto.</param>
    /// <returns>
    /// Retorna un <see cref="ActionResponse{T}"/> indicando si la operación fue exitosa.
    /// - <c>WasSuccess = true</c> si la entidad fue agregada correctamente, incluyendo la entidad en <c>Result</c>.
    /// - En caso de error, retorna un objeto con el detalle del fallo.
    /// </returns>
    /// <exception cref="DbUpdateException">
    /// Se lanza cuando ocurre un error de actualización en la base de datos.
    /// </exception>
    /// <exception cref="Exception">
    /// Se lanza cuando ocurre un error inesperado durante la operación.
    /// </exception>
    public virtual async Task<ActionResponse<T>> AddAsync(T entity)
    {
        _context.Add(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    /// <summary>
    /// Elimina de forma asincrónica una entidad del tipo <typeparamref name="T"/>
    /// identificada por su clave primaria (<paramref name="id"/>).
    /// </summary>
    /// <param name="id">El identificador único de la entidad a eliminar.</param>
    /// <returns>
    /// Retorna un <see cref="ActionResponse{T}"/> con el resultado de la operación:
    /// - <c>WasSuccess = true</c> si la entidad fue eliminada exitosamente.
    /// - <c>WasSuccess = false</c> y <c>Message = "ERR001"</c> si la entidad no fue encontrada.
    /// - <c>WasSuccess = false</c> y <c>Message = "ERR002"</c> si ocurrió un error al eliminar o guardar los cambios.
    /// </returns>
    /// <remarks>
    /// Este método captura excepciones generales. Para un mejor control, se recomienda
    /// especializar la captura de excepciones como <see cref="DbUpdateException"/>.
    /// </remarks>
    public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        try
        {
            _entity.Remove(row);
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSuccess = true,
            };
        }
        catch
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }

    /// <summary>
    /// Obtiene de manera asincrónica una entidad por su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la entidad a consultar.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> que contiene:
    /// - <c>WasSuccess = true</c> y la entidad encontrada en <c>Result</c>,
    ///   si existe una coincidencia en la base de datos.
    /// - <c>WasSuccess = false</c> y el mensaje de error "ERR001" si no se encuentra.
    /// </returns>
    ///
    public virtual async Task<ActionResponse<T>> GetAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row != null)
        {
            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = row
            };
        }
        return new ActionResponse<T>
        {
            WasSuccess = false,
            Message = "ERR001"
        };
    }

    /// <summary>
    /// Obtiene de manera asincrónica todas las entidades del tipo <typeparamref name="T"/>.
    /// </summary>
    /// <returns>
    /// Un <see cref="ActionResponse{IEnumerable{T}}"/> que contiene:
    /// - <c>WasSuccess = true</c> y la lista de entidades en <c>Result</c>.
    /// </returns>
    ///
    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
    {
        return new ActionResponse<IEnumerable<T>>
        {
            WasSuccess = true,
            Result = await _entity.ToListAsync()
        };
    }

    /// <summary>
    /// Actualiza de manera asincrónica una entidad existente del tipo <typeparamref name="T"/>
    /// en el contexto de base de datos.
    /// </summary>
    /// <param name="entity">La entidad que se desea actualizar.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> que contiene:
    /// - <c>WasSuccess = true</c> y la entidad actualizada en <c>Result</c>, si la operación es exitosa.
    /// - <c>WasSuccess = false</c> y un mensaje de error en <c>Message</c>,
    /// si ocurre una excepción durante la actualización.
    /// </returns>
    ///
    public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
    {
        try
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    /// <summary>
    /// Crea una respuesta de acción de tipo <see cref="ActionResponse{T}"/>
    /// cuando ocurre una excepción inesperada.
    /// </summary>
    /// <param name="exception">La excepción capturada durante la ejecución.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> con:
    /// - <c>WasSuccess = false</c>
    /// - <c>Message</c> conteniendo el mensaje de la excepción.
    /// </returns>
    ///
    private ActionResponse<T> ExceptionActionResponse(Exception exception)
    {
        return new ActionResponse<T>
        {
            WasSuccess = false,
            Message = exception.Message
        };
    }

    /// <summary>
    /// Crea una respuesta de acción de tipo <see cref="ActionResponse{T}"/>
    /// cuando ocurre un error relacionado con la actualización de la base de datos.
    /// </summary>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> con:
    /// - <c>WasSuccess = false</c>
    /// - <c>Message = "ERR003"</c> (código de error para identificar el problema de actualización).
    /// </returns>
    ///
    private ActionResponse<T> DbUpdateExceptionActionResponse()
    {
        return new ActionResponse<T>
        {
            WasSuccess = false,
            Message = "ERR003"
        };
    }
}