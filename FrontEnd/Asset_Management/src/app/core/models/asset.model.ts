export interface Asset {
  assetId: number;
  assetNo: string;
  assetName: string;
  assetCategory: string;
  assetModel?: string;
  manufacturingDate?: string;
  expiryDate?: string;
  assetValue: number;
  assetStatus: string;
  description?: string;
  imageUrl?: string;
}
