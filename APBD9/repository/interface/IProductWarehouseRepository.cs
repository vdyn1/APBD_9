namespace APBD_9.repository;
public interface IProductWarehouseRepository
{
    Task<bool> OrderAlreadyFulfilledAsync(int orderId);
    Task<int> InsertProductToWarehouseAsync(int warehouseId, int productId, int orderId, int amount, decimal price, DateTime createdAt);
}

