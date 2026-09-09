const firstPage: number = 1;
const defaultPageSize: number = 5;

export type PageRequestData = {
  pageNumber: number;
  pageSize: number;
};

export const initialPageRequest: PageRequestData = {
  pageNumber: firstPage,
  pageSize: defaultPageSize,
};
