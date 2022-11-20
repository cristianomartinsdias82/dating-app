import { QueryParams } from "./query-params";

export interface QueryCacheItem<T>
{
  queryParams: QueryParams,
  key: string,
  value: T
}
