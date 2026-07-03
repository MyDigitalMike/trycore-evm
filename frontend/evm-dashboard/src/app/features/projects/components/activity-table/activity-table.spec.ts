import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityTable } from './activity-table';

describe('ActivityTable', () => {
  let component: ActivityTable;
  let fixture: ComponentFixture<ActivityTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActivityTable],
    }).compileComponents();

    fixture = TestBed.createComponent(ActivityTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
