import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

import {
  Idea,
  Comment,
  CommentService,
  JwtTokenService,
  AlertService
} from '../../core';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html'
})
export class CommentListComponent implements OnInit {

  @Input() ideaId;
  @Input() creatorId;
  isSigned = false;
  comments: Comment[];
  initialLoading = true;
  public options: Object = {
    placeholderText: `This article is quite interesting`,
    height: 150,
    // Set the image upload parameter.
    requestHeaders: {
      Authorization: `Bearer ${this.tokenService.getToken()}`
    },
    imageUploadParam: 'File',
    imageUploadURL: '/api/file/upload',
    imageUploadMethod: 'POST',
    imageMaxSize: 1000000,
    imageAllowedTypes: ['jpeg', 'jpg', 'png', '.gif']
  }
  newCommentForm: FormGroup;
  loading = false;

  constructor(private fb: FormBuilder,
              private commentService: CommentService,
              private tokenService: JwtTokenService,
              private alertService: AlertService) { }

  ngOnInit() {
    this.isSigned = !this.tokenService.isTokenExpired();
    this.getComments();
    this.newCommentForm = this.fb.group({
      ideaId: [this.ideaId],
      message: ['']
    });
  }

  create() {
    if (this.newCommentForm.valid) {
      this.loading = true;
      this.commentService.createOrUpdate(this.newCommentForm.value)
        .finally(() => this.loading = false)
        .subscribe(
          result => {
            this.getComments();
            this.alertService.success("You've commented the idea");
          },
          err => console.log(err)
        )
    }
  }

  getComments() {
    this.commentService.getList(this.ideaId, this.creatorId)
    .finally(() => this.initialLoading = false)
    .subscribe(
     data => this.comments = data,
     err => console.log(err)
    );
  }

}
