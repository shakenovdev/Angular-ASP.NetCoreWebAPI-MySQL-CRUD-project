import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import {
  Idea,
  IdeaService
} from '../../core';

@Component({
  selector: 'app-idea-detail',
  templateUrl: './idea-detail.component.html'
})
export class IdeaDetailComponent implements OnInit {

  idea: Idea;

  constructor(private route: ActivatedRoute,
              private ideaService: IdeaService) { 
  }

  ngOnInit() {
    this.idea = this.route.snapshot.data['idea'];
  }

}
