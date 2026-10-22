  import { Component, computed, inject, OnInit, signal } from '@angular/core';
  import { DatePipe } from '@angular/common';
  import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
  import { AppointmentService } from '../../services/appointment-service';
  import { Appointment } from '../../models/appointment.model';

  @Component({
    selector: 'app-appointment',
    standalone: true,
    imports: [ReactiveFormsModule, DatePipe],
    templateUrl: './appointment.html',
    styleUrl: './appointment.scss'
  })
  export class AppointmentComponent implements OnInit {
    private appointmentService = inject(AppointmentService);
    private fb = inject(FormBuilder);

    // Estado de la pantalla, todo en signals
    appointments = signal<Appointment[]>([]);
    loading = signal(false);
    saving = signal(false);
    errorMessage = signal<string | null>(null);

    // Valor derivado: se recalcula solo cuando cambia appointments()
    total = computed(() => this.appointments().length);

    form = this.fb.nonNullable.group({
      patientName: ['', [Validators.required, Validators.maxLength(100)]],
      doctorName: ['', [Validators.required, Validators.maxLength(100)]],
      appointmentDate: ['', Validators.required]
    });

    ngOnInit() {
      this.loadAppointments();
    }

    loadAppointments() {
      this.loading.set(true);
      this.errorMessage.set(null);
      this.appointmentService.getAppointments().subscribe({
        next: (data) => {
          this.appointments.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          console.error('Error al cargar las citas desde .NET:', err);
          console.log('Detalles del error:', {
            status: err.status,
            statusText: err.statusText,
            url: err.url,
            message: err.message,
            error: err.error
          });
          this.errorMessage.set('No se pudieron cargar las citas.');
          this.loading.set(false);
        }

      });
    }

    save() {
      if (this.form.invalid) {
        this.form.markAllAsTouched();
        return;
      }

      const value = this.form.getRawValue();
      // El input datetime-local entrega "2026-11-05T11:00"; le agregamos los segundos
      const dto = {
        ...value,
        appointmentDate: value.appointmentDate.length === 16
          ? `${value.appointmentDate}:00`
          : value.appointmentDate
      };

      this.saving.set(true);
      this.errorMessage.set(null);
      this.appointmentService.createAppointment(dto).subscribe({
        next: () => {
          this.form.reset();
          this.saving.set(false);
          this.loadAppointments(); // recarga desde la API: la fuente de verdad es la base
        },
        error: (err) => {
          console.error('Error al crear la cita:', err);
          this.errorMessage.set(
            err.status === 400 ? 'Datos inválidos. Revisa el formulario.' : 'No se pudo guardar la cita.'
          );
          this.saving.set(false);
        }
      });
    }
  }