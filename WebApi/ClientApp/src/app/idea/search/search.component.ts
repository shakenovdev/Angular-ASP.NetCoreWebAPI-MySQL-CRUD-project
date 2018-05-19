import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Idea, IdeaService } from '../../core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html'
})
export class SearchComponent implements OnInit {

  searchValue: string;
  goingSearch = false;
  ideas: Idea[];
  loading = false;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private ideaService: IdeaService) { }

  ngOnInit() {
    this.ideas = this.route.snapshot.data['ideas'];
    this.route.params.subscribe(
      params => { this.searchValue = params['value']; }
    )
  }

  updateRoute() {
    this.goingSearch = true;
    this.search();
    this.router.navigate(['/idea/search', this.searchValue]);
  }

  search(continious = false) {
    if (continious)
      this.loading = true;
      
    const lastIdeaId = continious ? this.ideas.lastIdeaId() : null;
    this.ideaService.searchIdeas(this.searchValue, lastIdeaId)
    .finally(() => {
      this.goingSearch = false;
      this.loading = false;
    })
    .subscribe(
      data => {
        this.ideas = continious ? this.ideas.concat(data) : data;
      }
    );
  }

}
