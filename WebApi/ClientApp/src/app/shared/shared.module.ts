import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { 
  IdeaPreviewComponent,
  DateComponent
} from './components';

// third party libraries
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxErrorsModule } from '@ultimate/ngxerrors';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { FroalaViewModule } from 'angular-froala-wysiwyg';
import { MomentModule } from 'ngx-moment';
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { NgProgressModule, NgProgressConfig } from '@ngx-progressbar/core';
import { NgProgressRouterModule } from '@ngx-progressbar/router';

const progressBarConfig: NgProgressConfig = {
  spinner: false,
  color: '#f0ad4e',
  thick: true
};

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    MomentModule,
    AngularFontAwesomeModule,
    FroalaViewModule,
    NgbModule.forRoot(),
    NgxErrorsModule,
    InfiniteScrollModule,
    NgProgressModule.forRoot(progressBarConfig),
    NgProgressRouterModule
  ],
  declarations: [
    DateComponent,
    IdeaPreviewComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    MomentModule,
    AngularFontAwesomeModule,
    FroalaViewModule,
    NgxErrorsModule,
    NgbModule,
    InfiniteScrollModule,
    NgProgressModule,
    NgProgressRouterModule,
    DateComponent,
    IdeaPreviewComponent
  ]
})
export class SharedModule { }
