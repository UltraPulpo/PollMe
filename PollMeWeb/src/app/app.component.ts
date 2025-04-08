import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PollsListComponent } from './components/polls-list/polls-list.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PollsListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'PollMeWeb';
}
