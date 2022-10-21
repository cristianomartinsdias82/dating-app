import { QueryParams } from './../models/query-params';
import { PaginatedResult } from './../models/paginated-result';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EMPTY, generate, map, Observable, of,tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';

export interface IQueryCacheItem<T>
{
  queryParams: QueryParams,
  key: string,
  value: T
}

export class QueryCacheHelper
{
  static add<T>(queryParams: QueryParams, searchResults: T, cache: IQueryCacheItem<T>[]) {
    cache.push({ queryParams, key : this.generateKey(queryParams), value : searchResults });
  }

  static get<T>(queryParams: QueryParams, cache: IQueryCacheItem<T>[]) : T {
    return cache.find(x => x.key == this.generateKey(queryParams))?.value;
  }

  private static generateKey(queryParams: QueryParams): string {
    return Object.values(queryParams).join('|');
  }
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  members:Member[];
  paginatedMembers: PaginatedResult<Member[]> = new PaginatedResult([], null);

  cachedQueryResults: IQueryCacheItem<PaginatedResult<Member[]>>[] = [];

  constructor(private httpClient: HttpClient) { }

  /*
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
  */

  getPaginatedMembers(params: QueryParams): Observable<PaginatedResult<Member[]>> {

    const cachedResult = QueryCacheHelper.get<PaginatedResult<Member[]>>(params, this.cachedQueryResults);
    if (cachedResult) {
      return of(cachedResult);
    }

    return this.getPaginatedResult<Member[]>('users', params)
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
    /*
    const member = this.members?.find(m => m.id === id);
    if (member)
      return of(member);
    */

    if (!this.cachedQueryResults || this.cachedQueryResults.length === 0)
      return this.httpClient.get<Member>(`${environment.baseUrl}users/${id}`);

    return of(this.getMemberFromCacheBy((m:Member) => m.id === id));
  }

  getMemberByUserName(userName: string) {
    /*
    const member = this.members?.find(m => m.userName === userName);
    if (member) {
      return of(member);
    }
    */
    if (!this.cachedQueryResults || this.cachedQueryResults.length === 0)
      return this.httpClient.get<Member>(`${environment.baseUrl}users/username/${userName}`);

    return of(this.getMemberFromCacheBy((m:Member) => m.userName === userName));
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

  private getMemberFromCacheBy(expression): Member {

    return this.cachedQueryResults.reduce((arr, elem) => elem.value.result,[])
                                  .find(expression);
  }

  private getPaginatedResult<T>(endpoint: string, queryParams: QueryParams) {

    return this.httpClient
              .get<T>(
                `${environment.baseUrl}${endpoint}`,
                {
                  observe: 'response',
                  params: queryParams.getHttpParams()
                })
              .pipe(
                map(
                  response => new PaginatedResult<T>(
                                    response.body,
                                    JSON.parse(response.headers.get('Pagination')))
                )
              );
  }
}
