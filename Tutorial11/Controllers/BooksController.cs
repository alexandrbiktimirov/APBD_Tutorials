using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial11.DTOs;
using Tutorial11.Exceptions;
using Tutorial11.Services;

namespace Tutorial5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(IDbService dbService) : ControllerBase
    {
        private readonly IDbService _dbService = dbService;

        [HttpPost]
        public async Task<IActionResult> PostPrescription(PrescriptionDto prescriptionDto)
        {
            try
            {
                _dbService.PostPrescription(prescriptionDto);
            }
            catch (MedicationDoesNotExist e)
            {
                return NotFound(e.Message);
            }
            
            return Created("/", prescriptionDto);
        }
    }
}
