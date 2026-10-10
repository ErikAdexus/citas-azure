using System.ComponentModel.DataAnnotations;

namespace Citas.Api.DTOs
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "El nombre del paciente es obligatorio.")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del doctor es obligatorio.")]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        public DateTime AppointmentDate { get; set; }
    }
}
