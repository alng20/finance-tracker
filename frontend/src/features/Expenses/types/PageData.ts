const firstPage: number = 1;
const defaultPageSize: number = 20;

export type PageData = {
  pageNumber: number;
  pageSize: number;
};

export const initialPageData: PageData = {
  pageNumber: firstPage,
  pageSize: defaultPageSize,
};
