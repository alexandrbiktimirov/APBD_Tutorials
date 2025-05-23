using Microsoft.AspNetCore.Mvc;
using Tutorial11.Exceptions;
using Tutorial11.Services;

namespace Tutorial11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService patientService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatientData(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await patientService.GetPatientData(id, cancellationToken);
            return Ok(result);
        }
        catch (InvalidPatientIdException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (PatientDoesNotExistException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error occured", detail = ex.Message });
        }
    }
}