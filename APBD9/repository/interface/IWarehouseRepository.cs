namespace APBD_9.repository;

public interface IWarehouseRepository
{
    
    Task<bool> WarehouseExists(int id);
    
    
}