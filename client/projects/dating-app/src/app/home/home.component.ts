import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'dta-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private http: HttpClient) { }
  users: any;

  registrationMode = false;

  ngOnInit(): void {
  }

  registerToggle() {
    this.registrationMode = !this.registrationMode;
  }

  onRegistrationCancelled(event: boolean)
  {
    this.registrationMode = false;
  }

}
