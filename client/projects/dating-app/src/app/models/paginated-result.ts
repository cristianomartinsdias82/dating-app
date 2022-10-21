import { Pagination } from "./pagination";

export class PaginatedResult<T> {

  constructor(public result: T, public pagination: Pagination) {}
}
