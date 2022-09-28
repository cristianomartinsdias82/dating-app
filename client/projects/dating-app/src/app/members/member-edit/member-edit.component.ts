import { ToastrService } from 'ngx-toastr';
import { MembersService } from './../../services/members.service';
import { AccountService } from './../../services/account.service';
import { User } from './../../models/user';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../models/member';
import { take } from 'rxjs';
import { NgForm } from '@angular/forms';
import { CanLooseDataWhenUnsavedAndClosed } from '../../guards/can-loose-data-when-unsaved-and-closed';

@Component({
  selector: 'dta-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit, CanLooseDataWhenUnsavedAndClosed {

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.memberEditForm.dirty) {
      $event.returnValue = true;
    }
  }

  @ViewChild('memberEditForm') memberEditForm: NgForm;
  member:Member;
  user: User;

  constructor(
    public accountService: AccountService,
    private memberService: MembersService,
    private toastrService: ToastrService
  ) {
    this.accountService
        .currentUser$
        .pipe(take(1))
        .subscribe(user => this.user = user);
  }

  ngOnInit(): void {

    this.memberService
        .getMemberByUserName(this.user.userName)
        .subscribe(member => this.member = member);

  }

  onSubmit() {
      this.memberService
          .updateMemberProfile(this.member)
          .subscribe(x => {
            this.toastrService.success('Data updated successfully.');

            this.memberEditForm.reset(this.member);
          });
  }

  get knownAs() {
     return this.member?.knownAs + (this.memberEditForm?.dirty ? '*' : '');
  }

  canDeactivate() : boolean {
    return !this.memberEditForm.dirty;
  };
}
