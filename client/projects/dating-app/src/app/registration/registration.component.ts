import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../services/account.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dta-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  @Output() registrationCancelled = new EventEmitter<boolean>();

  model = { userName : '', password: '' };

  constructor(
    private accountService: AccountService,
    private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  get formIsValid()
  {
    return this.model.userName &&
           this.model.userName.trim().length > 0 &&
           this.model.password &&
           this.model.password.trim().length > 0;
  }

  onRegister() {
    this.accountService
        .register(this.model)
        .subscribe({
          next: response => {
            this.onCancel();
            this.toastrService.success('Registration successful.');
          },
          error: errorResponse => {
            console.log(errorResponse);
            this.toastrService.error(errorResponse.error.message);
          }
        });
  }

  onCancel() {
    this.registrationCancelled.emit(false);
  }
}
