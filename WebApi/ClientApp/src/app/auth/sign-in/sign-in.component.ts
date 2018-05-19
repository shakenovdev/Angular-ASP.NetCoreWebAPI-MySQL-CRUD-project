import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';

import { 
  AccountService,
  AlertService
} from '../../core';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html'
})
export class SignInComponent implements OnInit {

  signInForm: FormGroup
  loading = false
  returnUrl: string;

  constructor(private accountSerice: AccountService,
              private alertService: AlertService,
              private route : ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit() {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    this.signInForm = this.fb.group({
      email: ['', [Validators.required,
                   Validators.email]],
      password: ['', [Validators.required,
                      Validators.minLength(8)]]
    });
  }

  signIn() {
    if (this.signInForm.valid) {
      this.loading = true;
      this.accountSerice.signIn(this.signInForm.value)
      .finally(() => this.loading = false)
      .subscribe(
        res => {
          this.accountSerice.saveSignedUser(res);
          this.router.navigateByUrl(this.returnUrl);
        },
        err => {
          this.alertService.error(err.error);
        }
      )
    }
    
  }

}
