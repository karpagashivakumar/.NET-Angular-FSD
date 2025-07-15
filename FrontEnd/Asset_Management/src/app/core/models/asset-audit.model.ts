export interface AssetAudit {
  auditId?: number;
  assetId: number;
  employeeId: number;
  auditNotes?: string;
  auditStatus?: string;
  assetName?: string;
  employeeName?: string;
}