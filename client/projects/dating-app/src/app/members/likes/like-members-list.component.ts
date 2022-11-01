import { MembersService } from './../../services/members.service';
import { LikeTypes } from './like-types.enum';
import { Component, Input, OnInit } from '@angular/core';
import { Member } from '../../models/member';
import { Pagination } from '../../models/pagination';
import { QueryParams } from '../../models/query-params';
import { PaginatedResult } from '../../models/paginated-result';
import { AccountService } from '../../services/account.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'dta-like-members-list',
  templateUrl: './like-members-list.component.html',
  styleUrls: ['./like-members-list.component.scss']
})
export class LikeMembersListComponent implements OnInit {

  @Input() type: LikeTypes;
  members: Member[] = null;
  pagination: Pagination;
  pageNumber = 1;
  itemsPerPage = 5;

  constructor(
    private membersService: MembersService,
    private accountService: AccountService) { }

  ngOnInit(): void {
    this.loadList();
  }

  get queryParams(): QueryParams {

    return new QueryParams(
      this.pageNumber,
      this.itemsPerPage,
      "asc",
      "knownAs",
      null,
      null,
      null
    );

  }

  get emptyDataMessage(): string {
    return this.type === LikeTypes.followers ?
      "Don't be discouraged! Someone eventually will give you a like!" :
      "It's time for you to give someone a like!";
  }

  loadList() {

    // const delegate: (id: string, params: QueryParams) => Observable<PaginatedResult<Member[]>> = this.type === LikeTypes.followers ? this.membersService.getPaginatedLikerMembersFor : this.membersService.getPaginatedLikedMembersFor;

    if (this.type === LikeTypes.followers) {
      this.membersService.getPaginatedLikerMembersFor(
        this.accountService.getLoggedUser().id,
        this.queryParams)
        .subscribe((response:PaginatedResult<Member[]>) => {
          this.members = response.result;
          this.pagination = response.pagination;
        });
    } else {
      this.membersService.getPaginatedLikedMembersFor(
        this.accountService.getLoggedUser().id,
        this.queryParams)
        .subscribe((response:PaginatedResult<Member[]>) => {
          this.members = response.result;
          this.pagination = response.pagination;
        });
    }
  }

  onPageChanged(event: any) {
    this.loadList();
  }

  onItemsPerPageChanged(event: any) {
    this.loadList();
  }
}
