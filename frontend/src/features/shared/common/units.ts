// TODO: Get Units from backend

export type UnitDto = {
  id: string;
  name: string;
};

export const units: UnitDto[] = [
  "Undefined",
  "Piece",
  "G",
  "KG",
  "Milliliter",
  "Liter",
].map((unit) => ({ id: unit, name: unit }));
