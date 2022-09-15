import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { Member } from '../../models/member';

@Component({
  selector: 'dta-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss']
})
export class MembersListComponent implements OnInit {

  constructor(private memberService: MembersService) { }

  members: Member[];

  ngOnInit(): void {
    this.fetchMembersList();
  }

  fetchMembersList() {
    this
      .memberService
      .getMembers()
      .subscribe(result => this.members = result);
  }
}
