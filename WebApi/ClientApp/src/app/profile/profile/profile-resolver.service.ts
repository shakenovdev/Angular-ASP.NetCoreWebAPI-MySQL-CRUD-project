import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import {
  Profile,
  ProfileService,
  AlertService
} from '../../core';

@Injectable()
export class ProfileResolver implements Resolve<Observable<Profile>> {

  constructor(private router: Router,
              private profileService: ProfileService,
              private alertService: AlertService) { }
  
  resolve(route: ActivatedRouteSnapshot) {
    let username = route.paramMap.get('username');
    return this.profileService.getInfo(username)
      .catch(error => {
        this.alertService.errors(error.error, true);
        this.router.navigate(['/']);
        return Observable.of(null);
      });
  }
}
