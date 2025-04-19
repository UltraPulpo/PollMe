import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PollService } from './poll.service';

describe('PollService', () => {
  let service: PollService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PollService],
    });
    service = TestBed.inject(PollService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no outstanding HTTP requests
  });

  it('should fetch polls', () => {
    const mockPolls = [
      { id: 1, name: 'Poll 1', description: 'Description 1' },
      { id: 2, name: 'Poll 2', description: 'Description 2' },
    ];

    service.getPolls().subscribe((polls) => {
      expect(polls).toEqual(mockPolls);
    });

    const req = httpMock.expectOne('https://localhost:7272/api/polls');
    expect(req.request.method).toBe('GET');
    req.flush(mockPolls); // Provide mock response
  });
});
