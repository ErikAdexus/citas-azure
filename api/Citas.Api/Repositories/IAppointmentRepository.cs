using Citas.Api.Models;

namespace Citas.Api.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> AddAsync(Appointment appointment);
    }
}