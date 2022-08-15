import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'dta-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'DatingApp';
  users: any;

  constructor (private http: HttpClient) {}

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/users')
             .subscribe(
              {
                next: response => this.users = response,
                error: error => console.log(error)
              });
  }

  identify(user: any) {
    return user.id;
  }
}
