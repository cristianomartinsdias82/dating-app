import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../services/account.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';

@Injectable({providedIn: 'root'})
export class AuthenticationGuard implements CanActivate {

  constructor(
    private accountService: AccountService,
    private toastrService: ToastrService) {}

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user) {
          return true;
        }

        this.toastrService.info('You need to be authenticated to access this area.');
        return false;
      })
    );
  }

}
