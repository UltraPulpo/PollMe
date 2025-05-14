import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Poll } from '../../services/poll.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-polls-list',
  templateUrl: './polls-list.component.html',
  styleUrl: './polls-list.component.scss'
})
export class PollsListComponent {
  @Input() polls: Poll[] = [];
}
