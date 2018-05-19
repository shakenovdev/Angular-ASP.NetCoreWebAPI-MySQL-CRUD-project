import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './interceptors/token.interceptor';

import { 
  AccountService,
  AlertService,
  CommentService,
  IdeaService,
  JwtTokenService,
  ProfileService,
  TagService,
  FileService
} from './services';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    AccountService,
    AlertService,
    CommentService,
    IdeaService,
    JwtTokenService,
    ProfileService,
    TagService,
    FileService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}
  ],
  declarations: []
})
export class CoreModule { }
