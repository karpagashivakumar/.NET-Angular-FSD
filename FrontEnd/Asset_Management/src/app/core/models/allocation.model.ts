export interface AssetAllocation {
  allocationId: number;
  assetName: string;
  employeeName: string;
  allocatedDate: string;
  returnDate: string | null;
  allocationStatus: string;
  remarks: string;
}
