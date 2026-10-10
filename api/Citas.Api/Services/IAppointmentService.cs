using Citas.Api.DTOs;
using Citas.Api.Models;

namespace Citas.Api.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAppointmentsAsync();
        Task<Appointment> CreateAppointmentAsync(CreateAppointmentDto createDto);
    }
}