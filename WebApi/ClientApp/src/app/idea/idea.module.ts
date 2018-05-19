import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared';
import { IdeaDetailComponent } from './idea-detail/idea-detail.component';
import { IdeaNewComponent } from './idea-new/idea-new.component';
import { CommentComponent } from './comment/comment.component';
import { CommentListComponent } from './comment-list/comment-list.component';
import { IdeaDetailResolver } from './idea-detail/idea-detail-resolver.service';
import { SearchResolver } from './search/search-resolver.service';
import { AuthGuard } from '../core';
import { IdeaRoutingModule } from './idea-routing.module';

import { FroalaEditorModule } from 'angular-froala-wysiwyg';
import { TagInputModule } from 'ngx-chips';
import { IdeaComponent } from './idea/idea.component';
import { SearchComponent } from './search/search.component';

@NgModule({
  imports: [
    SharedModule,
    IdeaRoutingModule,
    FroalaEditorModule,
    TagInputModule
  ],
  declarations: [
    IdeaDetailComponent,
    IdeaNewComponent,
    CommentComponent,
    CommentListComponent,
    IdeaComponent,
    SearchComponent
  ],
  providers: [
    IdeaDetailResolver,
    SearchResolver,
    AuthGuard
  ]
})
export class IdeaModule { }
