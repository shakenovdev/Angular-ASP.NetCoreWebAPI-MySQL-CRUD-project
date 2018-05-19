import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import {
  Comment,
  Like,
  CommentService,
  AccountService,
  AlertService,
  JwtTokenService
} from '../../core';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html'
})
export class CommentComponent implements OnInit {

  @Input() comment: Comment;
  currentUser: any;
  removeLoading = false;
  subscription: Subscription;
  
  constructor(private commentService: CommentService,
              private accountService: AccountService,
              private alertService: AlertService) {
    this.subscription = this.accountService.changedSignedUser().subscribe(
      data => {
        if (data) {
          this.currentUser = data;
        } else {
          this.currentUser = null;
      }
    });        
  }

  ngOnInit() {
    this.currentUser = this.accountService.currentUser;
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  like(isLike: boolean) {
    const oldUserLike = this.comment.currentUserLike;
    const oldLikeCount = this.comment.likeCount;
    const multiplier = isLike ? 1 : -1;
    const newUserLike = oldUserLike == multiplier ? 0 : multiplier;

    this.comment.currentUserLike = newUserLike;
    this.comment.likeCount += newUserLike - oldUserLike;

    var data = new Like(this.comment.id, newUserLike);
    this.commentService.like(data).subscribe(
      data => { },
      err => {
        this.comment.currentUserLike = oldUserLike;
        this.comment.likeCount = oldLikeCount;
        this.alertService.errors(err.error);
      }
    )
  }

  removeOrRestore() {
    this.removeLoading = true;
    this.commentService.removeOrRestore(this.comment.id)
    .finally(() => this.removeLoading = false)
    .subscribe(
      data => {
        this.comment.isDeleted = !this.comment.isDeleted;

        if (this.comment.isDeleted)
          this.alertService.success("The comment was successfully removed!");
        else
          this.alertService.success("The comment was successfully restored!");
      },
      err => {
        this.alertService.errors(err.error);
      }
    )
  }

}
