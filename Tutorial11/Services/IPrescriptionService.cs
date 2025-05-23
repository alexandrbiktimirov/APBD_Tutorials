using Tutorial11.DTOs;
using Tutorial11.Models.DTOs;

namespace Tutorial11.Services;

public interface IPrescriptionService
{
    Task<int> CreateNewPrescriptionAsync(CreatePrescriptionDto createPrescriptionDto, CancellationToken cancellationToken);
}