using APBD_9.repository;
using Microsoft.Data.SqlClient;

namespace APBD_9.service;


public class ProductService : IProductService
{
    
    public readonly IProductRepository _repository;
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository repository, IProductRepository productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    public async Task ProductExists(int id)
    {
        if (!await _repository.ProductExistsAsync(id))
        {
            throw new ServiceException("Product does not exist",404);
        }
    }
    

    public async Task<decimal> GetPriceAsync(int productId)
    {
        var price = await _productRepository.GetPriceAsync(productId);
        return price ?? throw new Exception("Product price not found.");
    }
}
