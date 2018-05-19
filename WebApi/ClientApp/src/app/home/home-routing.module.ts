import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { HomeIdeaListResolver } from './home/home-idea-list-resolver.service';
import { DependenciesComponent } from './dependencies/dependencies.component';
import { ContactsComponent } from './contacts/contacts.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    resolve: {
      ideas: HomeIdeaListResolver
    }
  },
  {
    path: 'dependencies',
    component: DependenciesComponent
  },
  {
    path: 'contacts',
    component: ContactsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule {}