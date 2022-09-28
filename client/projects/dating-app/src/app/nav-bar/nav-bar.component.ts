import { AccountService } from './../services/account.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MembersService } from '../services/members.service';

@Component({
  selector: 'dta-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  loginModel: { userName : string, password: string } = { userName : '', password : '' };

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastrService: ToastrService) {}

  ngOnInit(): void {
  }

  login() {
    this.accountService
        .login(this.loginModel)
        .subscribe({
          next: () => {
            this.loginModel = { userName : '', password : '' };
            this.router.navigate(['/members']);
            this.toastrService.info('Login successful.');
          }
        });
  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['/']);
    this.toastrService.info('Logout successful.');
  }

  get formIsValid()
  {
    return this.loginModel.userName &&
           this.loginModel.userName.trim().length > 0 &&
           this.loginModel.password &&
           this.loginModel.password.trim().length > 0;
  }
}
