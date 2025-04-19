import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PollsListComponent } from './polls-list.component';
import { PollService } from '../../services/poll.service';
import { of } from 'rxjs';

describe('PollsListComponent', () => {
  let component: PollsListComponent;
  let fixture: ComponentFixture<PollsListComponent>;
  let mockPollService: jasmine.SpyObj<PollService>;

  beforeEach(async () => {
    mockPollService = jasmine.createSpyObj('PollService', ['getPolls']);
    mockPollService.getPolls.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [PollsListComponent],
      providers: [
        { provide: PollService, useValue: mockPollService },
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PollsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getPolls on PollService when initialized', () => {
    expect(mockPollService.getPolls).toHaveBeenCalled();
  });
});
