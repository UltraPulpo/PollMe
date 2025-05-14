import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PollsListComponent } from '../polls-list/polls-list.component';
import { Poll, PollService } from '../../services/poll.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, PollsListComponent],
  selector: 'app-polls-container',
  templateUrl: './polls-container.component.html',
  styleUrl: './polls-container.component.scss'
})
export class PollsContainerComponent {
  polls: Poll[] = [];
  selectedPoll: Poll | null = null; // Stores the selected poll object

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
        console.log(`Poll with ID ${id} deleted successfully.`);
        this.loadPolls(); // Refresh the local array by re-fetching from the backend
      },
      (error) => {
        console.error('Error deleting poll', error);
      }
    );
  }

  deleteSelectedPoll(): void {
    if (this.selectedPoll?.id !== undefined) {
      this.deletePoll(this.selectedPoll.id);
    }
  }

  onPollSelect(event: Event) {
    this.selectedPoll = this.polls.find(poll => poll.id === this.selectedPoll?.id) || null;
    console.log('Selected poll:', this.selectedPoll);
  }

  modifySelectedPoll(): void {
    if (!this.selectedPoll) return;

    // Prompt the user for a new name
    const newName = window.prompt(
      'Enter a new name for the poll:',
      this.selectedPoll.name // Pre-fill with the current name
    );

    // If the user cancels (returns null) or enters an empty name, do nothing
    if (newName === null || newName.trim() === '') {
      console.log('No changes made to the poll.');
      return;
    }

    // Update the selected poll
    const modifiedPoll: Poll = {
      ...this.selectedPoll,
      name: newName.trim() // Use the new name
    };

    this.pollService.updatePoll(modifiedPoll.id, modifiedPoll).subscribe(
      () => {
        console.log('Poll modified successfully:', modifiedPoll);
        this.loadPolls(); // Refresh the local array by re-fetching from the backend
      },
      (error) => {
        console.error('Error modifying poll:', error);
      }
    );
  }
}
