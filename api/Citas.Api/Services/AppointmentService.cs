using Citas.Api.DTOs;
using Citas.Api.Models;
using Citas.Api.Repositories;
using Microsoft.ApplicationInsights; // Necesario para TelemetryClient
using Microsoft.Extensions.Logging;

namespace Citas.Api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly ILogger<AppointmentService> _logger;
        private readonly TelemetryClient _telemetryClient;

        // Inyección de dependencias (el contenedor DI se encarga de resolver ILogger y TelemetryClient)
        public AppointmentService(
            IAppointmentRepository repository,
            ILogger<AppointmentService> logger,
            TelemetryClient telemetryClient)
        {
            _repository = repository;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
        {
            _logger.LogInformation("Consultando la lista completa de citas.");
            return await _repository.GetAllAsync();
        }

        public async Task<Appointment> CreateAppointmentAsync(CreateAppointmentDto createDto)
        {
            _logger.LogInformation("Iniciando la creación de una cita para el paciente: {PatientName}", createDto.PatientName);

            var appointment = new Appointment
            {
                PatientName = createDto.PatientName,
                DoctorName = createDto.DoctorName,
                AppointmentDate = createDto.AppointmentDate,
                Status = "Scheduled"
            };

            var createdAppointment = await _repository.AddAsync(appointment);

            // Registrar un evento personalizado en Application Insights
            _telemetryClient.TrackEvent("AppointmentCreated", new Dictionary<string, string>
            {
                { "AppointmentId", createdAppointment.Id.ToString() },
                { "DoctorName", createdAppointment.DoctorName },
                { "Status", createdAppointment.Status }
            });

            _logger.LogInformation("Cita creada exitosamente con ID: {AppointmentId}", createdAppointment.Id);

            return createdAppointment;
        }
    }
}