export interface ServiceRequest {
  serviceRequestId: number;
  assetNo: string;
  issueType: string;
  description: string;
  serviceStatus: string;
  employeeName: string;
  resolutionNotes?: string;
}

export interface ServiceRequestDto {
  assetId: number;
  assetNo: string;
  issueType: string;
  description: string;
}