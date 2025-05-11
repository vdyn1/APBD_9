namespace APBD_9.repository;

public interface IProductRepository
{
    Task<bool> ProductExistsAsync(int id);
    
    Task<decimal?> GetPriceAsync(int productId);
}
