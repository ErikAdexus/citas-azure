using Citas.Api.DTOs;
using Citas.Api.Models;
using Citas.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Citas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        // Inyección de dependencias del Servicio en el Controlador
        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _service.GetAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] CreateAppointmentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAppointment = await _service.CreateAppointmentAsync(createDto);

            return CreatedAtAction(nameof(GetAppointments), new { id = createdAppointment.Id }, createdAppointment);
        }
    }
}