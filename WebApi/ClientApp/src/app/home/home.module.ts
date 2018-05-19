import { ModuleWithProviders, NgModule } from '@angular/core';
import { SharedModule } from '../shared';
import { HomeRoutingModule } from './home-routing.module';
import { HomeIdeaListResolver } from './home/home-idea-list-resolver.service';
import { HomeComponent } from './home/home.component';

import { ContactsComponent } from './contacts/contacts.component';
import { DependenciesComponent } from './dependencies/dependencies.component';

@NgModule({
  imports: [
    SharedModule,
    HomeRoutingModule
  ],
  declarations: [
    HomeComponent,
    DependenciesComponent,
    ContactsComponent
  ],
  providers: [
    HomeIdeaListResolver
  ]
})
export class HomeModule { }
