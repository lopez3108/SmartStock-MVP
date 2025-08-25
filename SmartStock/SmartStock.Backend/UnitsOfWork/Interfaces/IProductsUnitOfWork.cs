using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork.Interfaces;

public interface IProductsUnitOfWork
{
    Task<IEnumerable<Product>> GetComboAsync(int categoryId);

    Task<ActionResponse<Product>> AddAsync(ProductDTO teamDTO);

    Task<ActionResponse<Product>> UpdateAsync(ProductDTO teamDTO);

    Task<ActionResponse<Product>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Product>>> GetAsync();
}