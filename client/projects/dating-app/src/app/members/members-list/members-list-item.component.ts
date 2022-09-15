import { Component, Input, OnInit } from '@angular/core';
import { Member } from '../../models/member';

@Component({
  selector: 'dta-members-list-item',
  templateUrl: './members-list-item.component.html',
  styleUrls: ['./members-list-item.component.scss']
})
export class MembersListItemComponent implements OnInit {

  @Input() memberData: Member;

  constructor() { }

  ngOnInit(): void {
  }

}
