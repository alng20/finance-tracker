interface ItemCommonData {
  name: string;
  categoryId: string;
  unit: string;
}

export type CreateItemData = ItemCommonData;

export type UpdateItemData = {
  id: string;
} & ItemCommonData;
