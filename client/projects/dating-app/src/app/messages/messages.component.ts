import { Component, OnInit, TemplateRef } from '@angular/core';
import { PaginatedResult } from './../models/paginated-result';
import { MessageTypes } from './../models/message-types';
import { Pagination } from './../models/pagination';
import { MessageService } from './../services/message.service';
import { Message } from '../models/message';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { QueryParams } from '../models/query-params';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'dta-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  pageSize = 5;
  pageNumber = 1;
  messageType = MessageTypes.Unread;
  modalRef?: BsModalRef;

  constructor(
    private messageService: MessageService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {

    this.messageService
        .getPaginatedMessages(this.queryParams)
        .subscribe({
          next: (response: PaginatedResult<Message[]>) => {
            this.messages = response.result;

            this.pagination = response.pagination;
          }
        });
  }

  get queryParams(): QueryParams {

    return new QueryParams(
      this.pageNumber,
      this.pageSize,
      null,
      null,
      null,
      null,
      null,
      this.messageType
    );

  }

  get messageCountLabel() {
    if (!this.pagination || this.pagination.itemCount === 0)
      return 'No messages.';

    return `${this.pagination.itemCount} message${(this.pagination.itemCount > 1 ? 's' : '')}.`;
  }

  onChanged(event?: any, pageNumber: number = 1) {
    this.pageNumber = this.pagination.pageNumber = pageNumber;
    this.loadMessages();
  }

  onPageChanged(event: PageChangedEvent): void {
    this.onChanged(event, event.page);
  }

  onDelete(message: Message, modalTemplate: TemplateRef<any>) {
    this.modalRef = this.modalService.show(modalTemplate, { class: 'modal-md', id: message.id });
  }

  onConfirmDeleteMessage() {
    const deletedByRecipient = this.messageType === MessageTypes.Inbox;
    this.messageService
        .deleteMessage(this.modalRef?.id.toString(), deletedByRecipient)
        .subscribe({
          next: (result) => {

            this.messages.splice(this.messages.findIndex((item) =>item.id === this.modalRef?.id), 1);
          },
          error: () => this.modalRef?.hide(),
          complete: () => this.modalRef?.hide()
        });
  }

  onDeclineDeleteMessage() {
    this.modalRef?.hide();
  }
}
