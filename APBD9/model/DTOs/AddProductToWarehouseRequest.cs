namespace APBD_9.model.DTOs;

public class AddProductToWarehouseRequest
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}




