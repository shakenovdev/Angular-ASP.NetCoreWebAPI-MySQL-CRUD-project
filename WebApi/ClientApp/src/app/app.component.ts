import { Component, OnInit } from '@angular/core';
import { AccountService } from './core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  constructor (private accountService: AccountService) { }

  ngOnInit() {
    // get new token if current token is about to die (expTime < 5 days)
    this.accountService.extendToken();
  }
}
