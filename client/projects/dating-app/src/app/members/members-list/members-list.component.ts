import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { Member } from '../../models/member';
import { Observable } from 'rxjs';

@Component({
  selector: 'dta-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss']
})
export class MembersListComponent implements OnInit {

  constructor(private memberService: MembersService) { }

  members$: Observable<Member[]>;

  ngOnInit(): void {
    this.members$ = this.memberService.getMembers();
  }
}
