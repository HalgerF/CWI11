using Tutorial5.Services;
using Microsoft.AspNetCore.Mvc;
using Tutorial5.Exceptions;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PatientsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var patient = await _service.GetPatientAsync(id);
            return Ok(patient);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}