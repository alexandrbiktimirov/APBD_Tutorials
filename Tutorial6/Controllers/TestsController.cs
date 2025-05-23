using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial6.Models;

namespace Tutorial6.Controllers
{
    // api/tests => [controller] = Tests
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        // GET api/tests
        [HttpGet]
        public IActionResult Get()
        {
            // var tests = Database.Tests;
            return Ok();
        }
        
        // GET api/tests/1
        // GET api/tests/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // var test = Database.Tests.FirstOrDefault(x => x.Id == id);
            return Ok();
        }

        // POST api/tests { "id": 4, "name": "Test4" }
        [HttpPost]
        public IActionResult Add()
        {
            // Database.Tests.Add(test);
            return Created();
        }
    }
}
