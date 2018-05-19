import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { 
  Idea,
  Profile,
  ProfileService,
  AlertService
} from '../../core';
import '../../shared/extensions';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent implements OnInit {

  profile: Profile;
  ideas: [Idea[]] = [[]];
  loadings = [false, false];

  constructor(private route: ActivatedRoute,
              private profileService: ProfileService,
              private alertService: AlertService) { }

  ngOnInit() {
    this.profile = this.route.snapshot.data['profile'];

    if (this.profile.sharedCount > 0)
      this.getList(0);
    if (this.profile.favoritedCount > 0)
      this.getList(1);
  }

  getList(kindId, continious=false) {
    this.loadings[kindId] = true;
    const lastIdeaId =  continious ? this.ideas[kindId].lastIdeaId() : null;
    this.profileService.getIdeaList(kindId, this.profile.name, lastIdeaId)
      .finally(() => this.loadings[kindId] = false)
      .subscribe(
        data => {
          this.ideas[kindId] = continious ? this.ideas[kindId].concat(data) : data;
        },
        err => {
          this.alertService.errors(err.error);
        }
    )
  }

}
