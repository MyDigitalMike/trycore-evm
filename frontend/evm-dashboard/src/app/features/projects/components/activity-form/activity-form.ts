import { Component, effect, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { ActivityResponse, CreateActivityRequest } from '../../../../core/models/activity.model';

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
  styleUrl: './activity-form.scss'
})
export class ActivityForm {
  activityToEdit = input<ActivityResponse | null>(null);

  activitySaved = output<CreateActivityRequest>();
  editCancelled = output<void>();

  private readonly formBuilder = new FormBuilder();

  form = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    bac: [null as number | null, [Validators.required, Validators.min(0)]],
    plannedProgressPercent: [null as number | null, [Validators.required, Validators.min(0), Validators.max(100)]],
    actualProgressPercent: [null as number | null, [Validators.required, Validators.min(0), Validators.max(100)]],
    actualCost: [null as number | null, [Validators.required, Validators.min(0)]]
  });

  constructor() {
    effect(() => {
      const activity = this.activityToEdit();

      if (!activity) {
        this.resetForm();
        return;
      }

      this.form.patchValue({
        name: activity.name,
        bac: activity.bac,
        plannedProgressPercent: activity.plannedProgressPercent,
        actualProgressPercent: activity.actualProgressPercent,
        actualCost: activity.actualCost
      });
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.getRawValue();

    const request: CreateActivityRequest = {
      name: formValue.name?.trim() ?? '',
      bac: Number(formValue.bac),
      plannedProgressPercent: Number(formValue.plannedProgressPercent),
      actualProgressPercent: Number(formValue.actualProgressPercent),
      actualCost: Number(formValue.actualCost)
    };

    this.activitySaved.emit(request);

    if (!this.activityToEdit()) {
      this.resetForm();
    }
  }

  cancelEdit(): void {
    this.resetForm();
    this.editCancelled.emit();
  }

  private resetForm(): void {
    this.form.reset({
      name: '',
      bac: null,
      plannedProgressPercent: null,
      actualProgressPercent: null,
      actualCost: null
    });
  }
}