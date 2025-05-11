using Microsoft.Data.SqlClient;

namespace APBD_9.repository;

public class WarehouseRepository : IWarehouseRepository
{
    
    public readonly IConfiguration _configuration;


    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public  async Task<bool> WarehouseExists(int id)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT 1 FROM Warehouse WHERE IdWarehouse = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        var result = await command.ExecuteScalarAsync();
        return result != null;
    }
}