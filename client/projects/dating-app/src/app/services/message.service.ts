import { CreateMessage } from './../models/create-message';
import { QueryParams } from './../models/query-params';
import { PaginatedResult } from './../models/paginated-result';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Message } from '../models/message';
import { getPaginatedResult } from '../helpers/pagination-helper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private httpClient: HttpClient) { }

  getMessagesThread(recipientUserId: string): Observable<Message[]> {
    return this.httpClient
              .get<Message[]>(`${environment.baseUrl}messages/thread/${recipientUserId}`)
  }

  getPaginatedMessages(
    params: QueryParams): Observable<PaginatedResult<Message[]>> {
      return getPaginatedResult(
              this.httpClient,
              'messages',
              params);
  }

  createMessage(message: CreateMessage) {
    return this.httpClient
              .post(`${environment.baseUrl}messages`, message);
  }

  deleteMessage(messageId: string, deletedByRecipient: boolean) {
    return this.httpClient
              .delete(`${environment.baseUrl}messages/${messageId}?deletedByRecipient=${deletedByRecipient}`);
  }
}
