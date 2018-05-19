import { Component, OnInit } from '@angular/core';

import {
  Alert,
  AlertType,
  AlertService
} from '../../../core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit {

  alerts: Alert[];

  constructor(private alertService: AlertService) { }

  ngOnInit() {
    this.alertService.getAlert().subscribe((alert: Alert) => {
      if (!alert) {
        // clear alerts when an empty alert is received
        this.alerts = [];
        return;
      } 
      // add alert
      this.alerts.push(alert);
      // close alert after 5s
      setTimeout(() => {
        this.removeAlert(alert);
      }, 5000);
    });
  }

  removeAlert(alert: Alert) {
    this.alerts = this.alerts.filter(x => x !== alert);
  }

  cssClass(alert: Alert) {
    if (!alert) {
        return;
    }

    switch (alert.type) {
      case AlertType.Success:
          return 'alert alert-success';
      case AlertType.Error:
          return 'alert alert-danger';
      case AlertType.Info:
          return 'alert alert-info';
      case AlertType.Warning:
          return 'alert alert-warning';
    }
  }

}
