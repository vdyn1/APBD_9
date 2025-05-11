using System.Data;
using APBD_9.model.DTOs;
using APBD_9.repository;
using Microsoft.Data.SqlClient;

namespace APBD_9.service;

public class WarehouseService : IWarehouseService
{
    private readonly IConfiguration _configuration;
    private readonly IWarehouseRepository _repository;
    private readonly IProductWarehouseRepository _productWarehouseRepository;

    private readonly IProductService _productService;
    private readonly IOrderService _orderService;


    public WarehouseService(IConfiguration configuration, IWarehouseRepository repository,
        IProductWarehouseRepository productWarehouseRepository, IProductService productService,
        IOrderService orderService)
    {
        _configuration = configuration;
        _repository = repository;
        _productWarehouseRepository = productWarehouseRepository;
        _productService = productService;
        _orderService = orderService;
    }

    public async Task WarehouseExists(int id)
    {
        if (!await _repository.WarehouseExists(id))
        {
            throw new ServiceException("Warehouse does not exist", 404);
        }
    }


    public async Task EnsureOrderNotAlreadyFulfilledAsync(int orderId)
    {
        if (await _productWarehouseRepository.OrderAlreadyFulfilledAsync(orderId))
        {
            throw new ServiceException($"Order with ID {orderId} has already been fulfilled.", 409);
        }
    }


    public async Task<int> AddProductToWarehouseAsync(AddProductToWarehouseRequest request)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await _productService.ProductExists(request.IdProduct);
            await WarehouseExists(request.IdWarehouse); 

            if (request.Amount <= 0)
                throw new ServiceException("Amount must be greater than 0.", 400);

            var orderId = await _orderService.GetOrderIdbyProductIdAndAmount(
                request.IdProduct, request.Amount, request.CreatedAt);

            if (await _productWarehouseRepository.OrderAlreadyFulfilledAsync(orderId))
                throw new ServiceException($"Order {orderId} already fulfilled", 409);

            await _orderService.SetOrderFulfilledAsync(orderId, request.CreatedAt);

            var price = await _productService.GetPriceAsync(request.IdProduct);

            var newId = await _productWarehouseRepository.InsertProductToWarehouseAsync(
                request.IdWarehouse, request.IdProduct, orderId,
                request.Amount, price * request.Amount,
                request.CreatedAt);

            await transaction.CommitAsync();
            return newId;
        }
        catch (ServiceException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ServiceException("Internal server error: " + ex.Message, 500);
        }
    }
    
    
    // процедура 
    public async Task<int> AddProductToWarehouseProcedure(AddProductToWarehouseRequest request)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        using var command = new SqlCommand("AddProductToWarehouse", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", request.Amount);
        command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

        try
        {
            var result = await command.ExecuteScalarAsync();
            if (result == null)
            {
                throw new ServiceException("Procedure returned null", 500);
            }

            return Convert.ToInt32(result);
        }
        catch (SqlException ex)
        {
            // Проверка ошибок SQL (уровень 18+ значит ошибка пользователя, не системы)
            throw new ServiceException($"[SQL] {ex.Message}", 400);
        }
        catch (Exception ex)
        {
            throw new ServiceException($"[ERROR] {ex.Message}", 500);
        }
    }

    
}