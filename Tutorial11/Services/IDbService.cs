using Tutorial11.DTOs;

namespace Tutorial11.Services;

public interface IDbService
{
    public Task<PrescriptionDto> PostPrescription(PrescriptionDto prescription);
}