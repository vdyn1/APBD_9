namespace APBD_9.service;

public interface IProductService
{
    Task ProductExists(int id);
    
    
    Task<decimal> GetPriceAsync(int productId);
    
    
    
}