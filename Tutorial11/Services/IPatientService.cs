using Tutorial11.DTOs;
using Tutorial11.Models.DTOs;

namespace Tutorial11.Services;

public interface IPatientService
{
    Task<PatientDataDto> GetPatientData(int id, CancellationToken cancellationToken);
}