export interface Appointment {
  id: number;
  patientName: string;
  doctorName: string;
  appointmentDate: string;
  status: string;
}

export interface CreateAppointmentDto {
  patientName: string;
  doctorName: string;
  appointmentDate: string;
}