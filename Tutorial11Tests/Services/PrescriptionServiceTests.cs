using Moq;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models;
using Tutorial11.Models.DTOs;
using Tutorial11.Services;
using Tutorial11.Repositories;

namespace Tutorial11.Tests.Services
{
    public class PrescriptionServiceTests
    {
        private static PrescriptionService Create(
            out Mock<IUnitOfWork> mockUow,
            out Mock<IPatientRepository> mockPatient,
            out Mock<IMedicamentRepository> mockMed,
            out Mock<IDoctorRepository> mockDoc,
            out Mock<IPrescriptionRepository> mockRx)
        {
            mockUow = new Mock<IUnitOfWork>();
            mockPatient = new Mock<IPatientRepository>();
            mockMed = new Mock<IMedicamentRepository>();
            mockDoc = new Mock<IDoctorRepository>();
            mockRx = new Mock<IPrescriptionRepository>();

            mockUow.Setup(u => u.PatientRepo).Returns(mockPatient.Object);
            mockUow.Setup(u => u.MedicamentRepo).Returns(mockMed.Object);
            mockUow.Setup(u => u.DoctorRepo).Returns(mockDoc.Object);
            mockUow.Setup(u => u.PrescriptionRepo).Returns(mockRx.Object);

            return new PrescriptionService(mockUow.Object);
        }

        [Fact]
        public async Task Throws_When_TooMany_Medicaments()
        {
            var service = Create(out _, out _, out _, out _, out _);

            var dto = new CreatePrescriptionDto
            {
                Medicaments = Enumerable.Range(0, 11)
                    .Select(i => new MedicamentDto { IdMedicament = i })
                    .ToList(),
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(1),
                Doctor = new DoctorDto { IdDoctor = 1 },
                Patient = new PatientDto { IdPatient = 1 }
            };

            await Assert.ThrowsAsync<TooMuchMedicationsException>(
                () => service.CreateNewPrescriptionAsync(dto, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Throws_When_DueDate_Not_After_Date()
        {
            var service = Create(out _, out _, out _, out _, out _);
            var dto = new CreatePrescriptionDto
            {
                Medicaments = [],
                Date = DateTime.Today,
                DueDate = DateTime.Today,
                Doctor = new DoctorDto { IdDoctor = 1 },
                Patient = new PatientDto { IdPatient = 1 }
            };

            await Assert.ThrowsAsync<InvalidPrescriptionDateException>(
                () => service.CreateNewPrescriptionAsync(dto, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Throws_When_Medicament_Not_Found()
        {
            var svc = Create(out _, out _, out var m, out _, out _);
            m.Setup(x => x.AllExistAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var dto = new CreatePrescriptionDto
            {
                Medicaments = new[] { new MedicamentDto { IdMedicament = 42 } }.ToList(),
                Date = DateTime.Today, DueDate = DateTime.Today.AddDays(1),
                Doctor = new DoctorDto { IdDoctor = 1 },
                Patient = new PatientDto { IdPatient = 1 }
            };

            await Assert.ThrowsAsync<MedicamentDoesNotExistException>(
                () => svc.CreateNewPrescriptionAsync(dto, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Throws_When_Doctor_Not_Found()
        {
            var svc = Create(out _, out _, out var m, out var d, out _);
            m.Setup(x => x.AllExistAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            d.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var dto = new CreatePrescriptionDto
            {
                Medicaments = [],
                Date = DateTime.Today, DueDate = DateTime.Today.AddDays(1),
                Doctor = new DoctorDto { IdDoctor = 99 },
                Patient = new PatientDto { IdPatient = 1 }
            };

            await Assert.ThrowsAsync<DoctorDoesNotExistException>(
                () => svc.CreateNewPrescriptionAsync(dto, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Commits_When_All_Valid()
        {
            var svc = Create(out var uow, out var pat, out var med, out var doc, out var rx);
            med.Setup(x => x.AllExistAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            doc.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            pat.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            pat.Setup(x => x.CreateNewAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(123);
            rx.Setup(x => x.CreateNewAsync(It.IsAny<Prescription>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(777);

            var dto = new CreatePrescriptionDto
            {
                Medicaments = new[] { new MedicamentDto { IdMedicament = 1, Dose = 5, Description = "OK" } }.ToList(),
                Date = DateTime.Today, DueDate = DateTime.Today.AddDays(1),
                Doctor = new DoctorDto { IdDoctor = 10 },
                Patient = new PatientDto { IdPatient = 55 }
            };

            var newId = await svc.CreateNewPrescriptionAsync(dto, CancellationToken.None);

            Assert.Equal(777, newId);

            uow.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            rx.Verify(x => x.AddManyMedicamentsToPrescriptionAsync(
                    It.Is<List<PrescriptionMedicament>>(l => l.Count == 1),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}