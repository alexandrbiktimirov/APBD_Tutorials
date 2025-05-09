using Tutorial9.Model.DTOs;

namespace Tutorial9.Services;

public interface IDbService
{
    Task DoSomethingAsync();
    Task ProcedureAsync();
    Task<bool> DoesProductExist(int idProduct);
    Task<bool> OrderExists(WarehouseDto dto);
}