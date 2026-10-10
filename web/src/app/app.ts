import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppointmentComponent } from './components/appointment/appointment';

@Component({
  imports: [AppointmentComponent],
  selector: 'app-root',
  standalone: true,
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
 title = 'web';
}
