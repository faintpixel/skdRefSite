import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitImagesComponent } from './submit-images.component';

describe('SubmitImagesComponent', () => {
  let component: SubmitImagesComponent;
  let fixture: ComponentFixture<SubmitImagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmitImagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmitImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
