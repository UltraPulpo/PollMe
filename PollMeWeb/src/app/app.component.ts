import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PollsContainerComponent } from './components/polls-container/polls-container.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PollsContainerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'PollMeWeb';
}
