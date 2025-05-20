using Microsoft.AspNetCore.Mvc;
using Tutorial5.DTOs;
using Tutorial5.Services;
using Tutorial5.Exceptions;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController(IPrescriptionService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostPrescription([FromBody] PostPrescriptionDto prescription)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var id = await service.CreatePrescriptionAsync(prescription);
            return CreatedAtAction(nameof(GetPrescription), new { id }, id);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetPrescription(int id)
    {
        return Ok(new { message = $"Prescription {id} placeholder" });
    }
}