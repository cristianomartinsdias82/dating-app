import { QueryParams } from './../../models/query-params';
import { Pagination } from './../../models/pagination';
import { MembersService } from './../../services/members.service';
import { Component, OnInit } from '@angular/core';
import { Member } from '../../models/member';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'dta-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss']
})
export class MembersListComponent implements OnInit {

  members: Member[];
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 5;
  gender = null;
  sortColumn = 'knownAs';
  sortDirection = 'asc';
  minAge?: number;
  maxAge?: number;

  get queryParams(): QueryParams {

    return new QueryParams(
      this.pageNumber,
      this.pageSize,
      this.sortDirection,
      this.sortColumn,
      this.minAge,
      this.maxAge,
      this.gender
    );

  }

  constructor(private memberService: MembersService) { }

  //members$: Observable<Member[]>;

  ngOnInit(): void {
    //this.members$ = this.memberService.getMembers();

    this.loadQueryFilters();

    this.loadMembersList();
  }

  loadQueryFilters() {

    const lastQueryFilter = this.memberService.getLastMemberQueryFilter();
    if (!lastQueryFilter)
      return;

    this.pageNumber = lastQueryFilter.pageNumber;
    this.pageSize = lastQueryFilter.pageSize;
    this.gender = lastQueryFilter.gender;
    this.sortColumn = lastQueryFilter.sortColumn;
    this.sortDirection = lastQueryFilter.sortDirection;
    this.minAge = lastQueryFilter.minAge;
    this.maxAge = lastQueryFilter.maxAge;

  }

  loadMembersList() {

    this.memberService.getPaginatedMembers(this.queryParams)
                      .subscribe({
                       next: (response) => {
                         this.members = response.result;
                         this.pagination = response.pagination;
                       }
                      });
  }

  get matchCountLabel() {
    if (this.pagination.itemCount === 0)
      return 'No match found.';

    return `Found ${this.pagination.itemCount} match${(this.pagination.itemCount > 1 ? 'es' : '')}.`;
  }

  onChanged(event?: any, pageNumber: number = 1) {
    this.pageNumber = this.pagination.pageNumber = pageNumber;
    this.loadMembersList();
  }

  onPageChanged(event: PageChangedEvent): void {
    this.onChanged(event, event.page);
  }
}
