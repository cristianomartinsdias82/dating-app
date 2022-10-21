import { HttpParams } from '@angular/common/http';

export class QueryParams {

  constructor(
    public pageNumber: number = 1,
    public pageSize: number = 5,
    public sortDirection = 'asc',
    public sortColumn = 'knownAs',
    public minAge?: number,
    public maxAge?: number,
    public gender?: number) {}

  public getHttpParams(): HttpParams {

    let httpParams = new HttpParams();

    if (this.minAge)
      httpParams = httpParams.append('minAge', this.minAge);

    if (this.maxAge)
      httpParams = httpParams.append('maxAge', this.maxAge);

    if (this.gender)
      httpParams = httpParams.append('gender', this.gender);

    if (this.pageNumber)
      httpParams = httpParams.append('pageNumber', this.pageNumber);

    if (this.pageSize)
      httpParams = httpParams.append('pageSize', this.pageSize);

    if (this.sortColumn)
      httpParams = httpParams.append('sortColumn', this.sortColumn);

    if (this.sortDirection)
      httpParams = httpParams.append('sortDirection', this.sortDirection);

    return httpParams;
  }
}
