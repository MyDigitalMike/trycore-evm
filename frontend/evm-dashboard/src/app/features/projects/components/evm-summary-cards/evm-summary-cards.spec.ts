import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EvmSummaryCards } from './evm-summary-cards';

describe('EvmSummaryCards', () => {
  let component: EvmSummaryCards;
  let fixture: ComponentFixture<EvmSummaryCards>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EvmSummaryCards],
    }).compileComponents();

    fixture = TestBed.createComponent(EvmSummaryCards);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
