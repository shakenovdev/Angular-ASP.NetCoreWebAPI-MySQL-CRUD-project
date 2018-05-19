import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';

import { 
  Idea, 
  Tag, 
  Like, 
  IdeaService, 
  AccountService,
  AlertService 
} from '../../../core';

@Component({
  selector: 'app-idea-preview',
  templateUrl: './idea-preview.component.html',
  styleUrls: ['./idea-preview.component.css']
})
export class IdeaPreviewComponent implements OnInit {

  @Input() idea: Idea;
  @Output() tagChanged = new EventEmitter<Tag>();
  @ViewChild('content') content: ElementRef;
  isCollapsed = true;

  constructor(private router: Router,
              private ideaService: IdeaService,
              private accountService: AccountService,
              private alertService: AlertService) { }

  ngOnInit() { }

  ngAfterViewInit() {
    // add READ MORE if height more than 350px
    setTimeout(() => {
      var height = this.content.nativeElement.offsetHeight;
      if (height > 350)
        this.isCollapsed = false;
    }, 0);
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

  selectTag(tag: Tag) {
    this.tagChanged.emit(tag);
  }

  goToComments() {
    this.router.navigate(['idea', this.idea.id], {fragment: "comments"});
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
  
}
