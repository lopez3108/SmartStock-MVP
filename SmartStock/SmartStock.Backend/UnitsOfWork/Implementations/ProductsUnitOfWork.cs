using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Backend.UnitsOfWork.Interfaces;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork.Implementations;

public class ProductsUnitOfWork : GenericUnitOfWork<Product>, IProductsUnitOfWork
{
    private readonly IProductsRepository _productsRepository;

    public ProductsUnitOfWork(IGenericRepository<Product> repository, IProductsRepository productsRepository) : base(repository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<ActionResponse<Product>> AddAsync(ProductDTO productDTO) => await _productsRepository.AddAsync(productDTO);

    public async Task<IEnumerable<Product>> GetComboAsync(int countryId) => await _productsRepository.GetComboAsync(countryId);

    public async Task<ActionResponse<Product>> UpdateAsync(ProductDTO productDTO) => await _productsRepository.UpdateAsync(productDTO);

    public override async Task<ActionResponse<Product>> GetAsync(int id) => await _productsRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync() => await _productsRepository.GetAsync();
}