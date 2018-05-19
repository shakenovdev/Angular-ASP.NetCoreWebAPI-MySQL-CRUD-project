import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { debounceTime } from 'rxjs/operator/debounceTime';
import { 
  Alert,
  AlertType
} from '../models';

@Injectable()
export class AlertService {

  private subject = new Subject<Alert>();
  private keepAfterRouteChange = false;

  constructor(private router: Router) {
    router.events.subscribe(event => {
      // alert action after route changed
      if (event instanceof NavigationStart) {
        if (this.keepAfterRouteChange) {
          this.keepAfterRouteChange = false;
        } else {
          this.clear();
        }
      }
    });
   }

  getAlert(): Observable<any> {
    return this.subject.asObservable();
  }

  alert(type: AlertType, message: string, keepAfterRouteChange = false) {
    this.keepAfterRouteChange = keepAfterRouteChange;
    this.subject.next(<Alert>{ type: type, message: message });
  }

  success(message: string, keepAfterRouteChange = false) {
    this.alert(AlertType.Success, message, keepAfterRouteChange);
  }

  error(message: string, keepAfterRouteChange = false) {
    this.alert(AlertType.Error, message, keepAfterRouteChange);
  }

  info(message: string, keepAfterRouteChange = false) {
    this.alert(AlertType.Info, message, keepAfterRouteChange);
  }

  warn(message: string, keepAfterRouteChange = false) {
    this.alert(AlertType.Warning, message, keepAfterRouteChange);
  }

  errors(messages: string[], keepAfterRouteChange = false) {
    messages.forEach(message => this.error(message, keepAfterRouteChange));
  }

  clear() {
    // clear alerts
    this.subject.next();
  }

}
