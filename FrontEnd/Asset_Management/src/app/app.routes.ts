import { Routes } from '@angular/router';
import { DashboardComponent } from './features/user/dashboard/dashboard';
import { LoginComponent } from './features/auth/login/login';
import { ProfileComponent } from './features/user/profile/profile';
import { AuthGuard } from './core/auth-guard';
import { AssetComponent } from './features/user/asset/asset';
import { AssetAllocationComponent } from './features/user/asset-allocation/asset-allocation';
import { ServiceRequestComponent } from './features/user/service-request/service-request';
import { AssetRequestComponent } from './features/user/asset-request/asset-request';
import { AssetAuditComponent } from './features/user/asset-audit/asset-audit';

export const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'asset', component: AssetComponent, canActivate: [AuthGuard] },
  { path: 'asset-allocation', component: AssetAllocationComponent, canActivate: [AuthGuard] },
  { path: 'service-request', component: ServiceRequestComponent, canActivate: [AuthGuard] },
  { path: 'asset-request', component: AssetRequestComponent, canActivate: [AuthGuard] },
  { path: 'asset-audit', component: AssetAuditComponent, canActivate: [AuthGuard]},
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
