using APBD_9.service;
using Microsoft.Data.SqlClient;

namespace APBD_9.repository;

public class OrderRepository : IOrderRepository
{
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public  async Task<int?> GetMatchingOrderIdAsync(int productId, int amount, DateTime createdAt)
    {
        
            await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
            await connection.OpenAsync();

            var cmd = new SqlCommand(@"
            SELECT TOP 1 IdOrder 
            FROM [Order]
            WHERE IdProduct = @IdProduct 
              AND Amount = @Amount
              AND FulfilledAt IS NULL
              AND CreatedAt < @CreatedAt
            ORDER BY CreatedAt", connection);

            cmd.Parameters.AddWithValue("@IdProduct", productId);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);

            var result = await cmd.ExecuteScalarAsync();
            return result == null ? null : (int)result;
        
    }
    
    
    
    public async Task UpdateFulfilledAtAsync(int orderId, DateTime fulfilledAt)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        var command = new SqlCommand(@"
            UPDATE [Order]
            SET FulfilledAt = @FulfilledAt
            WHERE IdOrder = @IdOrder", connection);

        command.Parameters.AddWithValue("@FulfilledAt", fulfilledAt);
        command.Parameters.AddWithValue("@IdOrder", orderId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
        {
            throw new Exception($"[ERROR] Order with ID {orderId} not found or update failed.");
        }
    }
    
}