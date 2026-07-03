import { Component, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { CreateActivityRequest } from '../../../../core/models/activity.model';

@Component({
  selector: 'app-activity-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './activity-form.html',
  styleUrl: './activity-form.scss',
})
export class ActivityForm {
  activityCreated = output<CreateActivityRequest>();

  private readonly formBuilder = new FormBuilder();

  form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    bac: [0, [Validators.required, Validators.min(0)]],
    plannedProgressPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    actualProgressPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    actualCost: [0, [Validators.required, Validators.min(0)]]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.activityCreated.emit(this.form.getRawValue());

    this.form.reset({
      name: '',
      bac: 0,
      plannedProgressPercent: 0,
      actualProgressPercent: 0,
      actualCost: 0
    });
  }
}
