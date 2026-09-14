const firstPage: number = 1;
const defaultPageSize: number = 10;

export type PageRequestData = {
  pageNumber: number;
  pageSize: number;
};

export const initialPageRequest: PageRequestData = {
  pageNumber: firstPage,
  pageSize: defaultPageSize,
};
