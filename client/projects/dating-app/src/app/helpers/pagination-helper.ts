import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../models/paginated-result';
import { QueryParams } from "../models/query-params";

export function getPaginatedResult<T>(
  httpClient: HttpClient,
  endpoint: string,
  queryParams: QueryParams) {

  return httpClient
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
