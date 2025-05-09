namespace Tutorial9.Model.DTOs;

public class WarehouseDto
{
    public required int IdProduct { get; set; }
    public required int IdWarehouse { get; set; }
    public required int Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
}