using Microsoft.Data.SqlClient;

namespace APBD_9.repository;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> OrderAlreadyFulfilledAsync(int orderId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await connection.OpenAsync();

        var command = new SqlCommand(@"
            SELECT 1 FROM Product_Warehouse WHERE IdOrder = @IdOrder", connection);
        command.Parameters.AddWithValue("@IdOrder", orderId);

        return await command.ExecuteScalarAsync() != null;
    }

    public async Task<int> InsertProductToWarehouseAsync(int warehouseId, int productId, int orderId, int amount, decimal price, DateTime createdAt)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await connection.OpenAsync();

        var command = new SqlCommand(@"
            INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
            VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);
            SELECT SCOPE_IDENTITY();", connection);

        command.Parameters.AddWithValue("@IdWarehouse", warehouseId);
        command.Parameters.AddWithValue("@IdProduct", productId);
        command.Parameters.AddWithValue("@IdOrder", orderId);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@Price", price);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}