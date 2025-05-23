using Microsoft.AspNetCore.Mvc;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Models.DTOs;
using Tutorial11.Services;

namespace Tutorial11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController(IPrescriptionService prescriptionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddNewPrescription([FromBody] CreatePrescriptionDto createPrescriptionDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var id = await prescriptionService.CreateNewPrescriptionAsync(createPrescriptionDto, cancellationToken);
            return CreatedAtAction(nameof(AddNewPrescription), new { id }, new CreatedResponseDto{ Id = id });
        }
        catch (Exception ex) when (ex is DoctorDoesNotExistException or MedicamentDoesNotExistException)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex) when (ex is InvalidPrescriptionDateException or TooMuchMedicationsException)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal Server Error occured", detail = ex.Message });
        }
    }
}