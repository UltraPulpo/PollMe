import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PollsListComponent } from '../polls-list/polls-list.component';
import { Poll, PollService } from '../../services/poll.service';

@Component({
  standalone: true,
  imports: [CommonModule, PollsListComponent],
  selector: 'app-polls-container',
  templateUrl: './polls-container.component.html',
  styleUrl: './polls-container.component.scss'
})
export class PollsContainerComponent {
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

  // Add a new poll
  addPoll(): void {
    const newPoll: Poll = {
      id: 0, // The backend will assign the ID
      name: `New Poll`,
      description: `Description for the new poll`
    };

    this.pollService.addPoll(newPoll).subscribe(
      (createdPoll: Poll) => {
        this.polls.push(createdPoll); // Add the new poll to the local array
      },
      (error) => {
        console.error('Error adding poll', error);
      }
    );
  }

  // Delete a poll by ID
  deletePoll(id: number): void {
    this.pollService.deletePoll(id).subscribe(
      () => {
        this.polls = this.polls.filter(poll => poll.id !== id); // Remove the poll from the local array
      },
      (error) => {
        console.error('Error deleting poll', error);
      }
    );
  }

}
