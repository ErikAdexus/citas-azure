using Citas.Api.Models;

namespace Citas.Api.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        // Lista en memoria temporal para simular la base de datos
        private static readonly List<Appointment> _appointments = new()
        {
            new Appointment { Id = 1, PatientName = "Juan Pérez", DoctorName = "Dra. Gomez", AppointmentDate = DateTime.Now.AddDays(1), Status = "Scheduled" },
            new Appointment { Id = 2, PatientName = "María López", DoctorName = "Dr. Torres", AppointmentDate = DateTime.Now.AddDays(2), Status = "Scheduled" }
        };

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            // Simulamos operación asíncrona (como si fuera Entity Framework a la BD)
            return await Task.FromResult(_appointments);
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            appointment.Id = _appointments.Count > 0 ? _appointments.Max(a => a.Id) + 1 : 1;
            _appointments.Add(appointment);
            return await Task.FromResult(appointment);
        }
    }
}