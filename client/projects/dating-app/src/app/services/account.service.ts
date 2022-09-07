import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';

import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { User } from '../models/user';
import { RegisterUser } from '../models/registerUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  private currentUserSource = new ReplaySubject<User>(1);

  currentUser$ = this.currentUserSource.asObservable();

  register(userRegistrationData: RegisterUser) {
    return this
      .http
      .post(`${environment.baseUrl}account/register`, userRegistrationData)
      .pipe(
        map((user: User) => {
          this.localPersistUserAndNotify(user);
        })
      );
  }

  login(loginModel: { userName: string, password: string }) {
    return this.http
               .post(`${environment.baseUrl}account/login`, loginModel)
               .pipe(
                  map((user: User) => {
                    this.localPersistUserAndNotify(user);
                  })
               );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  setCurrentUser(user: User)
  {
    this.currentUserSource.next(user);
  }

  private localPersistUserAndNotify(user: User, key: string = 'user') {
    if (user) {
      localStorage.setItem(key, JSON.stringify(user));

      this.currentUserSource.next(user);
    }
  }
}
