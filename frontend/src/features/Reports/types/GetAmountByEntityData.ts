export type GetAmountByEntityData<
  IdKey extends string,
  NameKey extends string,
> = {
  [K in IdKey | NameKey]: string;
} & {
  totalAmount: number;
};
