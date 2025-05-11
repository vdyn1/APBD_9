namespace APBD_9.repository;

public interface IOrderRepository
{
    Task<int?> GetMatchingOrderIdAsync(int productId, int amount, DateTime createdAt);
    
    Task UpdateFulfilledAtAsync(int orderId, DateTime fulfilledAt);


}