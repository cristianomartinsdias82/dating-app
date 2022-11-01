import { Member } from './../../models/member';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'dta-like-members-list-item',
  templateUrl: './like-members-list-item.component.html',
  styleUrls: ['./like-members-list-item.component.scss']
})
export class LikeMembersListItemComponent {

  @Input() member: Member;

}
