import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  members:Member[];

  constructor(private httpClient: HttpClient) { }

  getMembers() {

    if (!this.members || this.members.length === 0) {
      return this.httpClient
          .get<Member[]>(`${environment.baseUrl}users`)
          .pipe(
            tap<Member[]>(data => { this.members = data })
          );
    }

    return of(this.members);
  }

  getMemberById(id: string) {
    const member = this.members?.find(m => m.id === id);
    if (member)
      return of(member);

    return this.httpClient.get<Member>(`${environment.baseUrl}users/${id}`);
  }

  getMemberByUserName(userName: string) {
    const member = this.members?.find(m => m.userName === userName);
    if (member) {
      return of(member);
    }

    return this.httpClient.get<Member>(`${environment.baseUrl}users/username/${userName}`);
  }

  updateMemberProfile(member: Member) {
    return this
            .httpClient
            .put(`${environment.baseUrl}users`, member)
            .pipe(
              tap(x => {
                const index = this.members.findIndex(m => m.id === member.id);

                this.members[index] = member;
              })
            );
  }

  deletePhoto(memberId: string, photoId: string) {
    return this
            .httpClient
            .delete(`${environment.baseUrl}users/delete-photo/${photoId}`)
            .pipe(
              tap(x => {
                const member = this.members.find(x => x.id === memberId);

                if (member) {
                  member.photos.splice(member.photos.findIndex(x => x.id === photoId), 1);
                }
              })
            );
  }
}
