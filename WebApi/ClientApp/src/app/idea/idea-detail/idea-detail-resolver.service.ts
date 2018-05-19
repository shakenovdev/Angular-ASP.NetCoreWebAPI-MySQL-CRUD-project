import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import {
  Idea,
  IdeaService,
  AlertService
} from '../../core';

@Injectable()
export class IdeaDetailResolver implements Resolve<Observable<Idea>> {

  constructor(private router: Router,
              private ideaService: IdeaService,
              private alertService: AlertService) { }
  
  resolve(route: ActivatedRouteSnapshot) {
    let id = route.paramMap.get('id');
    return this.ideaService.getIdeaDetail(id)
      .catch(error => {
        this.alertService.errors(error.error, true);
        this.router.navigate(['/']);
        return Observable.of(null);
      });
  }
}
