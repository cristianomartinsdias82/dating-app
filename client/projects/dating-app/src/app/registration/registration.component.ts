import { RegisterUser } from './../models/registerUser';
import { Router } from '@angular/router';
import { matchValues } from './../validators/matchValues.validator';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../services/account.service';
import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'dta-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  @Output() registrationCancelled = new EventEmitter<boolean>();
  @ViewChild('dob') dob: ElementRef<HTMLInputElement>;

  modelStateErrors: string[];
  registerForm: FormGroup;
  dobMaxDate: Date;

  bsConfig?: Partial<BsDatepickerConfig>;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.formBuilder.group({
      gender: ['male'],
      knownAs: ['',  [Validators.required, Validators.minLength(2), Validators.maxLength(20)]],
      dob: ['',  [Validators.required]],
      city: ['',  [Validators.required]],
      country: ['',  [Validators.required]],
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)], [/*checkUserNameAvailability(this.accountService, 'userName')*/]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
      confirmPassword: ['',[Validators.required, matchValues('password')]],
      termsAccepted: [false, Validators.requiredTrue],
      photoUrl: ['./assets/user.png']
    });

    this.registerForm.controls['password'].valueChanges.subscribe(() => this.registerForm.controls['confirmPassword'].updateValueAndValidity());

    this.bsConfig = Object.assign({}, {
      containerClass : 'theme-orange',
      showClearButton: true,
      clearPosition: "right",
      dateInputFormat: "MM/DD/YYYY"
     });

    this.dobMaxDate = new Date();
    this.dobMaxDate.setFullYear(this.dobMaxDate.getFullYear() - 18);
  }

  onRegister() {

    const userRegistrationData = this.registerForm.value as RegisterUser;
    userRegistrationData.dob = this.getDate(this.dob.nativeElement.value)

    this.accountService
        .register(userRegistrationData)
        .subscribe({
          next: response => {
            this.router.navigateByUrl('/members');
            this.toastrService.success('Registration successful.');
          },
          error: errors => {
            if (Array.isArray(errors)) {
              this.modelStateErrors = errors;
            } else {
              this.modelStateErrors = null;
            }
          }
        });
  }

  onCancel() {
    this.registrationCancelled.emit(false);
  }

  private getDate(mmSddSyyyyFormattedDate:string) {
    return new Date(
      +(mmSddSyyyyFormattedDate.substr(6,4)),
      +(mmSddSyyyyFormattedDate.substr(0,2)) - 1,
      +(mmSddSyyyyFormattedDate.substr(3,2)));

  }
}
