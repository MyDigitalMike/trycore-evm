import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EvmChart } from './evm-chart';

describe('EvmChart', () => {
  let component: EvmChart;
  let fixture: ComponentFixture<EvmChart>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EvmChart],
    }).compileComponents();

    fixture = TestBed.createComponent(EvmChart);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
