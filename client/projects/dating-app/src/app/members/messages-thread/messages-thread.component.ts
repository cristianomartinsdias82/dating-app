import { Member } from './../../models/member';
import { NgForm } from '@angular/forms';
import { User } from './../../models/user';
import { Message } from './../../models/message';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { CreateMessage } from '../../models/create-message';

@Component({
  selector: 'dta-messages-thread',
  templateUrl: './messages-thread.component.html',
  styleUrls: ['./messages-thread.component.scss']
})
export class MessagesThreadComponent implements OnInit {

  @Input() messages: Message[];
  @Input() recipientMember: Member;
  @Output() messageCreated = new EventEmitter<CreateMessage>();
  loggedUser: User;

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.loggedUser = this.accountService.getLoggedUser();
  }

  getMessageItemHtmlClasses(message: Message): any {
    return {
      'alert-primary' : this.loggedUser.id === message.recipientId,
      'offset-sm-4' : this.loggedUser.id === message.recipientId,
      'alert-success' : this.loggedUser.id !== message.recipientId
    };
  }

  getWhoSaid(message: Message): string {
    return this.loggedUser.id == message.senderId ? 'You said:' : `${message.senderUserName} said:`;
  }

  onSendMessage(formData: NgForm) {

    this.messageCreated.emit({ recipientId: this.recipientMember.id, content: formData.controls['content'].value });

    formData.resetForm();
  }

  trackByMethod(idx: number, el: Message) {
    return el.id;
  }
}
