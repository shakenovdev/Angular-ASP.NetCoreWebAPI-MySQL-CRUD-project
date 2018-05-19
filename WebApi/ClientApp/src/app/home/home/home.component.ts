import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/finally';
import { 
  Idea, 
  Tag, 
  IdeaService, 
  TagService,
  PopularUser,
  ProfileService
} from '../../core';
import '../../shared/extensions';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  currentIdeas: Idea[];
  searchValue: string;
  goingSearch = false;
  popularTags: Tag[];
  popularUsers: PopularUser[];
  selectedTag: Tag = null;
  filterOptions = [
    {name: "Hot", id: 0},
    {name: "Best", id: 1},
    {name: "Recent", id: 2}
  ];
  periodOptions = [
    {name: "Today", id: 0},
    {name: "Week", id: 1},
    {name: "Month", id: 2},
    {name: "All time", id: 3}
  ];
  selectedFilterId = this.filterOptions[0].id;
  selectedPeriodId = this.periodOptions[3].id;
  loadings = {
    ideas: false,
    tags: false,
    users: false
  };

  constructor(private route: ActivatedRoute,
              private router: Router,
              private ideaService: IdeaService,
              private profileService: ProfileService,
              private tagService: TagService) { }

  ngOnInit() {
    this.currentIdeas = this.route.snapshot.data['ideas'];
    this.getPopularTags();
    this.getPopularUsers();
  }

  getList(continious = false) {
    this.loadings.ideas = true;
    if (!continious)
      this.currentIdeas = [];
    const lastIdeaId = this.currentIdeas.lastIdeaId();
    this.ideaService.getIdeas(this.selectedFilterId, this.selectedPeriodId, this.selectedTag, lastIdeaId)
      .finally(() => this.loadings.ideas = false)
      .subscribe(data => this.currentIdeas = continious ? this.currentIdeas.concat(data) : data);
  }

  getPopularTags() {
    this.loadings.tags = true;
    this.tagService.getPopular()
      .finally(() => this.loadings.tags = false)
      .subscribe(
        data => this.popularTags = data
      );
  }

  getPopularUsers() {
    this.loadings.users = true;
    this.profileService.getPopularUsers()
      .finally(() => this.loadings.users = false)
      .subscribe(
        data => this.popularUsers = data
      );
  }

  search() {
    this.goingSearch = true;
    this.router.navigate(['/idea/search', this.searchValue]).then(() => this.goingSearch = false);
  }

  selectTag(tag: Tag) {
    if (this.selectedTag != tag) {
      this.selectedTag = tag;
    } else {
      this.selectedTag = null;
    }

    this.getList();
  }

  removeTag() {
    this.selectedTag = null;
    this.getList();
  }

}
