import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private toastrService: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next
            .handle(request)
            .pipe(
              catchError(error => {

                if (error) {
                  const innerError = error.error;
                  switch(error.status) {
                    case 400:

                      if (innerError.errors) {
                        const validationErrors = error.error.errors;
                        const modelStateErrors = [];

                        for (const key in validationErrors) {
                          if (validationErrors[key]) {
                            for (const errorDetail in validationErrors[key]) {
                              if (errorDetail) {
                                modelStateErrors.push(validationErrors[key][errorDetail]);
                              }
                            }
                          }
                        }

                        throw modelStateErrors;
                      } else if (innerError.message) {
                        this.toastrService.error(innerError.message);
                      } else {
                        this.toastrService.error(error.statusText, error.status);
                      }

                      break;
                    case 401:
                      this.toastrService.error(error.statusText, error.status);
                      break;

                    case 403:
                      this.toastrService.error(error.statusText, error.status);
                      break;

                    case 404:

                      if (innerError.message) {
                        this.toastrService.error(innerError.message);
                      } else {
                      this.router.navigateByUrl('/not-found');
                      }
                      break;

                    case 500:
                      const extras: NavigationExtras = { state: { error: innerError } };
                      this.router.navigateByUrl('/error', extras);
                      break;

                    case 0:
                    default:
                        this.toastrService.error('Unexpected application behavior. Please, do let the application support/administrator know about this occurrence. We apologize for the inconvenient.');
                        console.log(error);
                        break;
                  }
                }

                return throwError(() => error);
              })
            );
  }
}
