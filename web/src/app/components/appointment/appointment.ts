import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppointmentService } from '../../services/appointment-service';
import { Appointment } from '../../models/appointment.model';

@Component({
  selector: 'app-appointment',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './appointment.html',
  styleUrl: './appointment.scss'
})
export class AppointmentComponent implements OnInit {
  private appointmentService = inject(AppointmentService);
  
  // Signal para almacenar la lista de citas
  appointments = signal<Appointment[]>([]);

  ngOnInit() {
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentService.getAppointments().subscribe({
      next: (data) => this.appointments.set(data),
      error: (err) => console.error('Error al cargar las citas desde .NET:', err)
    });
  }
}