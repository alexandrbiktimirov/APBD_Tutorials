using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tutorial11.Controllers;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models.DTOs;
using Tutorial11.Services;

namespace Tutorial11.Tests.Controllers;

public class PatientsControllerTests
{
    private static PatientsController CreatePatientController(out Mock<IPatientService> serviceMock)
    {
        serviceMock = new Mock<IPatientService>();
        return new PatientsController(serviceMock.Object);
    }
    
    [Fact]
    public async Task GetPatientData_Returns200_EverythingFine()
    {
        var controller = CreatePatientController(out var serviceMock);
        
        var patientData = new PatientDataDto();
        
        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(patientData);

        var result = await controller.GetPatientData(1, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetPatientData_Returns400_InvalidPatientId()
    {
        var controller = CreatePatientController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidPatientIdException("Patient ID must be positive integer"));

        var result = await controller.GetPatientData(1, CancellationToken.None);
        
        var brResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, brResult.StatusCode);
    }

    [Fact]
    public async Task GetPatientData_Returns404_PatientNotFound()
    {
        var controller = CreatePatientController(out var serviceMock);

        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new PatientDoesNotExistException("Patient with this ID was not found"));

        var result = await controller.GetPatientData(1, CancellationToken.None);
        
        var nfResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, nfResult.StatusCode);
    }

    [Fact]
    public async Task GetPatientData_Returns500_InternalServerError()
    {
        var controller = CreatePatientController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.GetPatientData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Internal server error occured"));

        var result = await controller.GetPatientData(1, CancellationToken.None);
        
        var scResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, scResult.StatusCode);
    }
}