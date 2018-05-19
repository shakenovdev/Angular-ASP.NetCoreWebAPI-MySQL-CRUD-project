import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { 
  Idea, 
  Tag, 
  Like, 
  IdeaService, 
  AccountService,
  AlertService
} from '../../core';

@Component({
  selector: 'app-idea',
  templateUrl: './idea.component.html',
  styleUrls: ['./idea.component.css']
})
export class IdeaComponent implements OnInit {

  @Input() idea: Idea;
  currentUser: any;
  fragment;
  removeLoading = false;
  subscription: Subscription;
  
  constructor(private route: ActivatedRoute,
              private router: Router,
              private ideaService: IdeaService,
              private accountService: AccountService,
              private alertService: AlertService) {
    this.subscription = this.accountService.changedSignedUser().subscribe(
      data => {
        if (data) {
          this.currentUser = data;
        } else {
          this.currentUser = null;
        }
      }
    )
  }

  ngOnInit() {
    this.currentUser = this.accountService.currentUser;
    
    this.route.fragment.subscribe(fragment => { this.fragment = fragment; });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      if (this.fragment)
        document.querySelector('#' + this.fragment).scrollIntoView();
      else
        window.scrollTo(0,0);
    }, 0);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  like(isLike: boolean) {
    const oldUserLike = this.idea.currentUserLike;
    const oldLikeCount = this.idea.likeCount;
    const multiplier = isLike ? 1 : -1;
    const newUserLike = oldUserLike == multiplier ? 0 : multiplier;

    this.idea.currentUserLike = newUserLike;
    this.idea.likeCount += newUserLike - oldUserLike;

    var data = new Like(this.idea.id, newUserLike);
    this.ideaService.like(data).subscribe(
      data => { },
      err => {
        this.idea.currentUserLike = oldUserLike;
        this.idea.likeCount = oldLikeCount;
        this.alertService.errors(err.error);
      }
    )
  }

  setFavorite() {
    this.idea.currentUserIsFavorited = !this.idea.currentUserIsFavorited;

    this.ideaService.setFavorite(this.idea.id).subscribe(
      data => {
        if (this.idea.currentUserIsFavorited)
          this.alertService.success("The idea was added to favorites");
        else
          this.alertService.success("The idea was removed from favorites");
      },
      err => {
        this.idea.currentUserIsFavorited = !this.idea.currentUserIsFavorited;
        this.alertService.errors(err.error);
      }
    )
  }

  removeOrRestore() {
    this.removeLoading = true;
    this.ideaService.removeOrRestore(this.idea.id)
    .finally(() => this.removeLoading = false)
    .subscribe(
      data => {
        this.idea.isDeleted = !this.idea.isDeleted;

        if (this.idea.isDeleted)
          this.alertService.success('The idea was successfully removed!');
        else
          this.alertService.success('The idea was successfully restored!');
      },
      err => {
        this.alertService.errors(err.error);
      }
    )
  }
  
}
