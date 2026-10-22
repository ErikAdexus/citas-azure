using Citas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Citas.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(e =>
            {
                e.Property(a => a.PatientName).HasMaxLength(100).IsRequired();
                e.Property(a => a.DoctorName).HasMaxLength(100).IsRequired();
                e.Property(a => a.Status).HasMaxLength(30).IsRequired();

                // Fechas fijas: HasData debe ser determinista, con DateTime.Now
                // cada migración detectaría "cambios" falsos.
                e.HasData(
                    new Appointment { Id = 1, PatientName = "Juan Pérez", DoctorName = "Dra. Gomez", AppointmentDate = new DateTime(2026, 11, 1, 9, 0, 0), Status = "Scheduled" },
                    new Appointment { Id = 2, PatientName = "María López", DoctorName = "Dr. Torres", AppointmentDate = new DateTime(2026, 11, 2, 10, 0, 0), Status = "Scheduled" }
                );
            });
        }
    }
}