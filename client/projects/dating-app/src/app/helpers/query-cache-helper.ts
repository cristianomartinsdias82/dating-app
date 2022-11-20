import { QueryCacheItem } from "../models/query-cache-item";
import { QueryParams } from "../models/query-params";

export class QueryCacheHelper
{
  static add<T>(queryParams: QueryParams, searchResults: T, cache: QueryCacheItem<T>[]) {
    cache.push({ queryParams, key : this.generateKey(queryParams), value : searchResults });
  }

  static get<T>(queryParams: QueryParams, cache: QueryCacheItem<T>[]) : T {
    return cache.find(x => x.key == this.generateKey(queryParams))?.value;
  }

  private static generateKey(queryParams: QueryParams): string {
    return Object.values(queryParams).join('|');
  }
}
