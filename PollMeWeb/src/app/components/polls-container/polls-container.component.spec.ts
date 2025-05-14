import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PollsContainerComponent } from './polls-container.component';
import { PollService } from '../../services/poll.service';
import { of } from 'rxjs';

describe('PollsContainerComponent', () => {
  let component: PollsContainerComponent;
  let fixture: ComponentFixture<PollsContainerComponent>;
  let mockPollService: jasmine.SpyObj<PollService>;

  beforeEach(async () => {
    mockPollService = jasmine.createSpyObj('PollService', ['getPolls']);
    mockPollService.getPolls.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [PollsContainerComponent],
      providers: [
        { provide: PollService, useValue: mockPollService },
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PollsContainerComponent);
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
