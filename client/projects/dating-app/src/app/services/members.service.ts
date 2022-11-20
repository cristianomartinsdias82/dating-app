import { QueryParams } from './../models/query-params';
import { PaginatedResult } from './../models/paginated-result';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of,tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';
import { QueryCacheItem } from '../models/query-cache-item';
import { QueryCacheHelper } from '../helpers/query-cache-helper';
import { getPaginatedResult } from '../helpers/pagination-helper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  members:Member[];
  paginatedMembers: PaginatedResult<Member[]> = new PaginatedResult([], null);
  cachedQueryResults: QueryCacheItem<PaginatedResult<Member[]>>[] = [];

  constructor(private httpClient: HttpClient) { }

  getPaginatedMembers(params: QueryParams): Observable<PaginatedResult<Member[]>> {

    const cachedResult = QueryCacheHelper.get<PaginatedResult<Member[]>>(params, this.cachedQueryResults);
    if (cachedResult) {
      return of(cachedResult);
    }

    return getPaginatedResult<Member[]>(this.httpClient, 'users', params)
               .pipe(
                map(response => {

                  if (response && response.result.length > 0)
                    QueryCacheHelper.add<PaginatedResult<Member[]>>(params, response, this.cachedQueryResults);

                  return response;

                })
               );
  }

  getLastMemberQueryFilter(): QueryParams {
    if (!this.cachedQueryResults || this.cachedQueryResults.length === 0)
      return null;

    return this.cachedQueryResults[this.cachedQueryResults.length - 1].queryParams;
  }

  getMemberById(id: string) {

    const member = this.getMemberFromCacheBy((m:Member) => m.id === id);

    if (!member)
      return this.httpClient.get<Member>(`${environment.baseUrl}users/${id}`);

    return of(member);
  }

  getMemberByUserName(userName: string) {

    const member = this.getMemberFromCacheBy((m:Member) => m.userName === userName);
    if (!member) {
      return this.httpClient.get<Member>(`${environment.baseUrl}users/username/${userName}`);
    }

    return of(member);
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
                if (this.members) {
                  const member = this.members.find(x => x.id === memberId);

                  if (member) {
                    member.photos.splice(member.photos.findIndex(x => x.id === photoId), 1);
                  }
                }
              })
            );
  }

  getPaginatedLikedMembersFor(id: string, queryParams: QueryParams): Observable<PaginatedResult<Member[]>> {

    return getPaginatedResult<Member[]>(this.httpClient, `users/${id}/liked-users`, queryParams);

  }

  getPaginatedLikerMembersFor(id: string, queryParams: QueryParams): Observable<PaginatedResult<Member[]>> {

    return getPaginatedResult<Member[]>(this.httpClient, `users/${id}/liker-users`, queryParams);

  }

  toggleLike(id: string) {

    return this.httpClient.put(`${environment.baseUrl}users/toggle-like`, { likedUserId : id });

  }

  private getMemberFromCacheBy(expression): Member {

    return this.cachedQueryResults.reduce((arr, elem) => elem.value.result,[])
                                  .find(expression);
  }
}
