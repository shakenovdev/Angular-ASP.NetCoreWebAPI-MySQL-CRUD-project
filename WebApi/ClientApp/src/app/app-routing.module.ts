import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';

const routes: Routes = [
    {
      path: 'idea',
      loadChildren: './idea/idea.module#IdeaModule'
    },
    {
      path: 'auth',
      loadChildren: './auth/auth.module#AuthModule'
    },
    {
      path: 'profile',
      loadChildren: './profile/profile.module#ProfileModule'
    }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    // preload all modules; optionally we could
    // implement a custom preloading strategy for just some
    // of the modules (PRs welcome 😉)
    preloadingStrategy: PreloadAllModules
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {} 