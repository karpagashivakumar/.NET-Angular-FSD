export interface AssetRequest {
  requestId?: number;
  assetCategory: string;
  requestDescription: string;
  requestStatus?: string;
  requestDate?: Date;
  employeeName?: string;
  rejectionReason?: string;
}
