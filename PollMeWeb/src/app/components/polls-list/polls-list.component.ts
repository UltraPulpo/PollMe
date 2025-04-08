import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Poll, PollService } from '../../services/poll.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-polls-list',
  templateUrl: './polls-list.component.html',
  styleUrl: './polls-list.component.scss'
})
export class PollsListComponent implements OnInit {
  polls: Poll[] = [];

  constructor(private pollService: PollService) { }

  ngOnInit(): void {
    this.loadPolls();
  }

  // Fetch all products from the API
  loadPolls(): void {
    this.pollService.getPolls().subscribe(
      (data: Poll[]) => {
        this.polls = data;
      },
      (error) => {
        console.error('Error fetching polls', error);
      }
    );
  }
}
