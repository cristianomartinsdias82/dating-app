import { MembersService } from './../services/members.service';
import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberDetailsResolver implements Resolve<Member> {

  constructor(private memberService: MembersService){}

  resolve(route: ActivatedRouteSnapshot): Observable<Member> {
    return this.memberService.getMemberByUserName(route.paramMap.get('id'));
  }

}
