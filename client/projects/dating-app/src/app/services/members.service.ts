import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  constructor(private httpClient: HttpClient) { }

  getMembers() {
    return this.httpClient.get<Member[]>(`${environment.baseUrl}users`);
  }

  getMemberById(id: string) {
    return this.httpClient.get<Member>(`${environment.baseUrl}users/${id}`);
  }

  getMemberByUserName(userName: string) {
    return this.httpClient.get<Member>(`${environment.baseUrl}users/username/${userName}`);
  }
}
