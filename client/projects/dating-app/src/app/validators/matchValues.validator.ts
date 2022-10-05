import { AbstractControl, ValidatorFn } from "@angular/forms";

export const matchValues = (matchTo: string): ValidatorFn =>
  (control: AbstractControl) => {
    return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching : true };
  };
