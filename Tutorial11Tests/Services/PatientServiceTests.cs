using Moq;
using Tutorial11.Exceptions;
using Tutorial11.Models;
using Tutorial11.Services;
using Tutorial11.Repositories;

namespace Tutorial11.Tests.Services;

public class PatientServiceTests
{
    private static PatientService Create(out Mock<IUnitOfWork> uow,
        out Mock<IPatientRepository> mockPat,
        out Mock<IPrescriptionRepository> mockRx)
    {
        uow = new Mock<IUnitOfWork>();
        mockPat = new Mock<IPatientRepository>();
        mockRx = new Mock<IPrescriptionRepository>();

        uow.Setup(x => x.PatientRepo).Returns(mockPat.Object);
        uow.Setup(x => x.PrescriptionRepo).Returns(mockRx.Object);

        return new PatientService(uow.Object);
    }

    [Fact]
    public async Task Throws_When_Patient_Not_Exist()
    {
        var svc = Create(out _, out var pat, out _);
        pat.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<PatientDoesNotExistException>(
            () => svc.GetPatientData(5, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Returns_Correct_DTO_When_Patient_Exists()
    {
        var svc = Create(out _, out var pat, out var rx);
        var now = DateTime.UtcNow;
        var patient = new Patient
        {
            IdPatient = 1, FirstName = "John", LastName = "Doe", BirthDate = now.AddYears(-30)
        };
        var doctor = new Doctor
        {
            IdDoctor = 1, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@gmail.com"
        };
            
        var prescription = new Prescription
        {
            IdPrescription = 1,
            Date = now, DueDate = now.AddDays(7),
            IdPatient = 1, IdDoctor = 1,
            Doctor = doctor,
            PrescriptionMedicaments = new List<PrescriptionMedicament>
            {
                new()
                {
                    IdMedicament = 1,
                    Dose = 50,
                    Details = "Details",
                    Medicament = new Medicament { IdMedicament = 1, Name = "Medicament", Description = "Description", Type = "Some type" }
                }
            }
        };

        pat.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        pat.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        rx.Setup(x => x.GetAllByPatientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync([prescription]);

        var dto = await svc.GetPatientData(1, CancellationToken.None);

        Assert.Equal(1, dto.IdPatient);
        Assert.Single(dto.Prescriptions);
        var rxDto = dto.Prescriptions[0];
        Assert.Equal(1, rxDto.IdPrescription);
        Assert.Equal("Jane", rxDto.Doctor.FirstName);
        Assert.Single(rxDto.Medicaments);
        Assert.Equal(50, rxDto.Medicaments[0].Dose);
    }
}