//https://www.thisdot.co/blog/using-custom-async-validators-in-angular-reactive-forms
import { map, Observable, debounceTime, filter, distinctUntilChanged, switchMap } from 'rxjs';
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { AccountService } from "../services/account.service";

export const checkUserNameAvailability = (accountService: AccountService, targetId: string): AsyncValidatorFn => (control: AbstractControl): Observable<ValidationErrors> => {

  const a = control.valueChanges.pipe(
            distinctUntilChanged((prev, cur) => prev !== cur),
            debounceTime(1500),
            filter((v: any) => v.trim().length >= 3),
            switchMap((v:any) => {
              return accountService
                      .checkUserNameAvailability(v)
                      .pipe(
                        map((isAvailable) => !isAvailable ? { userNameAlreadyTaken : true } : null)
                      )
            }));

  a.subscribe();

  return a;
};
