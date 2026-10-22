using Citas.Api.Data;
using Citas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Citas.Api.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Appointment>> GetAllAsync() =>
            await _context.Appointments.AsNoTracking().OrderBy(a => a.AppointmentDate).ToListAsync();

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();   // el Id lo genera SQL Server
            return appointment;
        }
    }
}