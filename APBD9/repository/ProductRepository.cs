using Microsoft.Data.SqlClient;

namespace APBD_9.repository;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT 1 FROM Product WHERE IdProduct = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        var result = await command.ExecuteScalarAsync();
        return result != null;
    }

    public async Task<decimal?> GetPriceAsync(int productId)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @Id", connection);
        command.Parameters.AddWithValue("@Id", productId);

        var result = await command.ExecuteScalarAsync();
        return result != null ? (decimal?)result : null;
    }
}
