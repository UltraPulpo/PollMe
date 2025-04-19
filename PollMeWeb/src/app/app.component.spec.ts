import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { PollService } from './services/poll.service';
import { of } from 'rxjs';

describe('AppComponent', () => {
  let mockPollService: jasmine.SpyObj<PollService>;

  beforeEach(async () => {
    mockPollService = jasmine.createSpyObj('PollService', ['getPolls']);
    mockPollService.getPolls.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        { provide: PollService, useValue: mockPollService },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have the 'PollMeWeb' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('PollMeWeb');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, PollMeWeb');
  });
});
