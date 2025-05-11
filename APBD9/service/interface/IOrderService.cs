namespace APBD_9.service;

public interface IOrderService
{
    Task<int> GetOrderIdbyProductIdAndAmount(int productId, int amount, DateTime date);
    
    Task SetOrderFulfilledAsync(int orderId, DateTime fulfilledAt);

}