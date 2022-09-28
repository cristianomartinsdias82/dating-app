import { AccountService } from './services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'dta-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  users: any;

  constructor (private accountService: AccountService) {}

  ngOnInit() {
    this.accountService.loadUserData();
  }

  identify(user: any) {
    return user.id;
  }
}
