import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import {
  AccountService
} from '../../../core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

  collapse: boolean = true;
  searchValue: string;
  goingSearch = false;
  currentUser;
  subscription: Subscription;
  
  constructor(private router: Router,
              private accountService: AccountService) { 
    this.subscription = this.accountService.changedSignedUser().subscribe(
      data => {
        if (data) {
          this.currentUser = data;
        } else {
          this.currentUser = null;
      }
    });
  }

  ngOnInit() {
    this.currentUser = this.accountService.currentUser;
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  signOut() {
    this.accountService.signOut();
  }

  search() {
    this.goingSearch = true;
    this.router.navigate(['/idea/search', this.searchValue]).then(() => 
      {
        this.goingSearch = false;
        this.collapse = true;
      }
    );
  }

}
