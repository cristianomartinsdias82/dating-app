import { AccountService } from './services/account.service';
import { Component, OnInit } from '@angular/core';
import { User } from './models/user';

@Component({
  selector: 'dta-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  users: any;

  constructor (private accountService: AccountService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user:User = JSON.parse(localStorage.getItem('user'));

    this.accountService.setCurrentUser(user);
  }

  identify(user: any) {
    return user.id;
  }
}
