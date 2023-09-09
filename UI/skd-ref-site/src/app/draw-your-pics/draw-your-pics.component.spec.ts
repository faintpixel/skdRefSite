import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrawYourPicsComponent } from './draw-your-pics.component';

describe('DrawYourPicsComponent', () => {
  let component: DrawYourPicsComponent;
  let fixture: ComponentFixture<DrawYourPicsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DrawYourPicsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrawYourPicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
