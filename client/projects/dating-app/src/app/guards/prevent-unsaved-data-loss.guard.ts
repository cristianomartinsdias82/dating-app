import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { CanLooseDataWhenUnsavedAndClosed } from './can-loose-data-when-unsaved-and-closed';

@Injectable({providedIn:'root'})
export class PreventUnsavedDataLossGuard implements CanDeactivate<CanLooseDataWhenUnsavedAndClosed> {

  canDeactivate(component: CanLooseDataWhenUnsavedAndClosed) : boolean | Observable<boolean> {

    return component.canDeactivate();

  }

}
