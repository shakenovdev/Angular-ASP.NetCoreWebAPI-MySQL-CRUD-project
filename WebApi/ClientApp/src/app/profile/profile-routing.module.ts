import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { SettingsComponent } from './settings/settings.component';
import { ProfileResolver } from './profile/profile-resolver.service';
import { SettingsResolver } from './settings/settings-resolver.service';
import { AuthGuard } from '../core';


const routes: Routes = [
  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [AuthGuard],
    resolve: {
      user: SettingsResolver
    }
  },
  {
    path: ':username',
    component: ProfileComponent,
    resolve: {
      profile: ProfileResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule {}