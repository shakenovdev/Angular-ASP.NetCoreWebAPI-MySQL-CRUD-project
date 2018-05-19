import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import 'rxjs/add/operator/finally';

import { 
  AccountService,
  AlertService
} from '../../core';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html'
})
export class SignUpComponent implements OnInit {

  signUpForm: FormGroup;
  loading = false;

  constructor(private accountService: AccountService,
              private alertService: AlertService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit() {
    this.signUpForm = this.fb.group({
      name: ['', [Validators.required, 
                  Validators.minLength(3)]],
      email: ['', [Validators.required,
                   Validators.email]],
      password: ['', [Validators.required,
                      Validators.minLength(8)]]
    })
  }

  signUp() {
    if (this.signUpForm.valid) {
      this.loading = true;
      this.accountService.signUp(this.signUpForm.value)
        .finally(() => this.loading = false)
        .subscribe(
          result => {
            this.alertService.success(result + " (approximately 1-2 minutes)", true);
            this.router.navigate(['/']);
          },
          err => {
            this.alertService.error(err.error);
          })
    }
  }
}
