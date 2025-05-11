using APBD_9.model.DTOs;

namespace APBD_9.service;

public interface IWarehouseService
{
    Task WarehouseExists(int id);

    Task<int> AddProductToWarehouseAsync(AddProductToWarehouseRequest request);


    Task EnsureOrderNotAlreadyFulfilledAsync(int orderId);


    Task<int> AddProductToWarehouseProcedure(AddProductToWarehouseRequest request);


}