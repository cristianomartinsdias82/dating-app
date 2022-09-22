import { Observable } from "rxjs";

export interface CanLooseDataWhenUnsavedAndClosed {
  canDeactivate: () => boolean | Observable<boolean>;
}
