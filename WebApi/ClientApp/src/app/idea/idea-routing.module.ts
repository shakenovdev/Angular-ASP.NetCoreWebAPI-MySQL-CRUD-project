import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IdeaNewComponent } from './idea-new/idea-new.component';
import { IdeaDetailComponent } from './idea-detail/idea-detail.component';
import { IdeaDetailResolver } from './idea-detail/idea-detail-resolver.service';
import { AuthGuard } from '../core';
import { SearchComponent } from './search/search.component';
import { SearchResolver } from './search/search-resolver.service';

const routes: Routes = [
    {
        path: 'new',
        component: IdeaNewComponent,
        canActivate: [AuthGuard]
    },
    {
        path: ':id',
        component: IdeaDetailComponent,
        resolve: {
            idea: IdeaDetailResolver
        }
    },
    {
        path: 'search/:value',
        component: SearchComponent,
        resolve: {
            ideas: SearchResolver
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdeaRoutingModule {}