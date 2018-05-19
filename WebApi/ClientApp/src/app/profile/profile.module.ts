import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared';
import { ProfileComponent } from './profile/profile.component';
import { SettingsComponent } from './settings/settings.component';
import { ProfileResolver } from './profile/profile-resolver.service';
import { SettingsResolver } from './settings/settings-resolver.service';
import { AuthGuard } from '../core';
import { ProfileRoutingModule } from './profile-routing.module';

@NgModule({
  imports: [
    SharedModule,
    ProfileRoutingModule
  ],
  declarations: [
    ProfileComponent, 
    SettingsComponent
  ],
  providers: [
    ProfileResolver,
    SettingsResolver,
    AuthGuard
  ]
})
export class ProfileModule { }
