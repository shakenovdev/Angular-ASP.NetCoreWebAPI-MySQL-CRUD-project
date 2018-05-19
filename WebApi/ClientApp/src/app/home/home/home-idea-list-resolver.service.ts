import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import { Idea, IdeaService } from '../../core';

@Injectable()
export class HomeIdeaListResolver implements Resolve<Observable<Idea[]>> {

  constructor(private ideaService: IdeaService) { }

  resolve(route: ActivatedRouteSnapshot) {
    return this.ideaService.getIdeas()
      .catch(error => {
        return Observable.of(null);
      });
  }
}
