import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Poll {
  id: number;
  name: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class PollService {
  private apiUrl = 'https://localhost:7272/api/polls';

  constructor(private http: HttpClient) { }

  // Fetch all polls
  getPolls(): Observable<Poll[]> {
    return this.http.get<Poll[]>(this.apiUrl);
  }

  // Fetch a poll by ID
  getPoll(id: number): Observable<Poll> {
    return this.http.get<Poll>(`${this.apiUrl}/${id}`);
  }

  // Create a new poll
  addPoll(poll: Poll): Observable<Poll> {
    return this.http.post<Poll>(this.apiUrl, poll);
  }

  // Update an existing poll
  updatePoll(id: number, poll: Poll): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, poll);
  }

  // Delete a poll
  deletePoll(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
