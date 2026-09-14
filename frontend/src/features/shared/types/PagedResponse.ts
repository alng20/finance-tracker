export interface PagedResponse<T, MetadataType = undefined> {
  page: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  data: T[];
  metadata?: MetadataType;
}
