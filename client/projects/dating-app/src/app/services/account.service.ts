import { Photo } from './../models/photo';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';

import { Injectable } from '@angular/core';
import { BehaviorSubject, map, of, ReplaySubject } from 'rxjs';
import { User } from '../models/user';
import { RegisterUser } from '../models/registerUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) {}

  private currentUserSource = new ReplaySubject<User>(1);
  private currentUserMainPhotoUrlSource = new BehaviorSubject<string>('./assets/user.png');

  currentUser$ = this.currentUserSource.asObservable();
  userMainPhotoUrl$ = this.currentUserMainPhotoUrlSource.asObservable();

  register(userRegistrationData: RegisterUser) {

    return this
      .http
      .post(`${environment.baseUrl}account/register`, userRegistrationData)
      .pipe(
        map((user: User) => {
          user.photoUrl = './assets/user.png';

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
    if (user) {
      this.currentUserSource.next(user);
      this.currentUserMainPhotoUrlSource.next(user.photoUrl);
    }
  }

  setMainPhoto(photo: Photo) {
    return this.http
               .put(`${environment.baseUrl}users/set-main-photo/${photo.id}`, null)
               .pipe(
                map(_ => {
                  const user = this.getLoggedUser();
                  user.photoUrl = photo.url;

                  this.localPersistUserAndNotify(user);
                })
               );
  }

  getLoggedUser(key: string = 'user'): User {
    return JSON.parse(localStorage.getItem(key));
  }

  loadUserData() {
    this.setCurrentUser(this.getLoggedUser());
  }

  checkUserNameAvailability(userName: string) {
    return this.http.post<boolean>(`${environment.baseUrl}account/check-username-availability/${userName}`, null);
  }

  private localPersistUserAndNotify(user: User, key: string = 'user') {
    if (user) {
      localStorage.setItem(key, JSON.stringify(user));

      this.currentUserSource.next(user);
      this.currentUserMainPhotoUrlSource.next(user.photoUrl);
    }
  }
}
