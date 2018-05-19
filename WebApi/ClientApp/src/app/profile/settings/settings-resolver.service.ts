import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import {
  ProfileService,
  AlertService,
  UserSettings
} from '../../core';

@Injectable()
export class SettingsResolver implements Resolve<Observable<UserSettings>> {

  constructor(private router: Router,
              private profileService: ProfileService,
              private alertService: AlertService) { }
  
  resolve() {
    return this.profileService.getUserSettings()
      .catch(error => {
        this.alertService.errors(error.error, true);
        this.router.navigate(['/']);
        return Observable.of(null);
      });
  }
}
