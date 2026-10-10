using Citas.Api.DTOs;
using Citas.Api.Models;
using Citas.Api.Repositories;

namespace Citas.Api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        // Inyección de dependencias del Repositorio en el Servicio
        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Appointment> CreateAppointmentAsync(CreateAppointmentDto createDto)
        {
            var appointment = new Appointment
            {
                PatientName = createDto.PatientName,
                DoctorName = createDto.DoctorName,
                AppointmentDate = createDto.AppointmentDate,
                Status = "Scheduled"
            };

            return await _repository.AddAsync(appointment);
        }
    }
}