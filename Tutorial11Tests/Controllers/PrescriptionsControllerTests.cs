using Microsoft.AspNetCore.Mvc;
using Moq;
using Tutorial11.Controllers;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models.DTOs;
using Tutorial11.Services;

namespace Tutorial11.Tests.Controllers;

public class PrescriptionsControllerTests
{

    private static PrescriptionsController CreateMockController(out Mock<IPrescriptionService> serviceMock)
    {
        serviceMock = new Mock<IPrescriptionService>();
        return new PrescriptionsController(serviceMock.Object);
    }
    
    [Fact]
    public async Task AddNewPrescription_Returns201_SuccessfulCreation()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(777);

        var result = await controller.AddNewPrescription(new CreatePrescriptionDto(), CancellationToken.None);

        var cResult = Assert.IsType<CreatedAtActionResult>(result);
        var value = Assert.IsType<CreatedResponseDto>(cResult.Value);
        Assert.Equal(777, value.Id);
        Assert.Equal(201, cResult.StatusCode);
    }

    [Fact]
    public async Task AddNewPrescription_Returns404_SomethingNotFound()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DoctorDoesNotExistException("Doctor does not exist"));
        
        var result = await controller.AddNewPrescription(new CreatePrescriptionDto(), CancellationToken.None);

        var nfResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, nfResult.StatusCode);
    }

    [Fact]
    public async Task AddNewPrescription_Returns409_ConflictExists()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidPrescriptionDateException("Invalid date for prescription"));
        
        var result = await controller.AddNewPrescription(new CreatePrescriptionDto(), CancellationToken.None);

        var cResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, cResult.StatusCode);
    }

    [Fact]
    public async Task AddNewPrescription_Returns500_InternalServerError()
    {
        var controller = CreateMockController(out var serviceMock);
        
        serviceMock
            .Setup(s => s.CreateNewPrescriptionAsync(It.IsAny<CreatePrescriptionDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Internal server error occurred"));
        
        var result = await controller.AddNewPrescription(new CreatePrescriptionDto(), CancellationToken.None);

        var exResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, exResult.StatusCode);
    }
}