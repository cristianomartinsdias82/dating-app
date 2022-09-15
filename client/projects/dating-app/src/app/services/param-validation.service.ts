import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ParamValidationService {

  constructor() { }

  uuidRegex = /^[0-9a-fA-F]{8}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{12}$/gi;

  uuidIsValid(input: string) {

    return this.uuidRegex.test(input);
  }
}
