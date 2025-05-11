using APBD_9.repository;

namespace APBD_9.service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<int> GetOrderIdbyProductIdAndAmount(int productId,
        int amount,
        DateTime date)
    {
        var orderId = await _orderRepository.GetMatchingOrderIdAsync(productId, amount, date);
        if (orderId == null)
        {
            throw new ServiceException(
                $"No matching unfulfilled order found for product {productId} with amount {amount}.",
                404
            );
        }

        return orderId.Value;
    }

    public async Task SetOrderFulfilledAsync(int orderId, DateTime fulfilledAt)
    {
        await _orderRepository.UpdateFulfilledAtAsync(orderId, fulfilledAt);
    }
    
}