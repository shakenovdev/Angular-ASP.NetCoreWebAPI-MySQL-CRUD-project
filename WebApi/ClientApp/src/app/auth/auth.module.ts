import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared';
import { AuthRoutingModule } from './auth-routing.module';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';


@NgModule({
  imports: [
    SharedModule,
    AuthRoutingModule
  ],
  declarations: [
    SignInComponent,
    SignUpComponent
  ]
})
export class AuthModule { }
