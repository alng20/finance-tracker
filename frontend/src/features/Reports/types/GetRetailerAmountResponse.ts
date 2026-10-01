export type GetRetailerAmountResponse = {
  fromDate: string | null;
  toDate: string | null;
  totalAmount: number;
  amounts: RetailerAmount[];
};

export type RetailerAmount = {
  retailerId: string | null;
  retailerName: string;
  totalAmount: number;
};
