import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import 'rxjs/add/operator/finally';

import {
  IdeaService, 
  JwtTokenService
} from '../../core';

@Component({
  selector: 'app-idea-new',
  templateUrl: './idea-new.component.html'
})
export class IdeaNewComponent implements OnInit {

  public options: Object = {
    placeholderText: `Put your hand on a hot stove for a minute, and it seems like an hour. 
                      Sit with a pretty girl for an hour, and it seems like a minute. That's relativity
                       &copy; Albert Einstein`,
    heightMin: 300,
    heightMax: 600,
    // Set the image upload parameter.
    requestHeaders: {
      Authorization: `Bearer ${this.tokenService.getToken()}`
    },
    imageUploadParam: 'File',
    imageUploadURL: '/api/file/upload',
    imageUploadMethod: 'POST',
    imageMaxSize: 1000000,
    imageAllowedTypes: ['jpeg', 'jpg', 'png', '.gif']
  }
  newIdeaForm: FormGroup;
  loading = false;
  
  constructor(private fb: FormBuilder,
              private ideaService: IdeaService,
              private tokenService: JwtTokenService,
              private router: Router) { }

  ngOnInit() {
    this.newIdeaForm = this.fb.group({
      title: ['', [Validators.required, 
                   Validators.minLength(10)]],
      tags: [[], [Validators.minLength(2),
                  Validators.maxLength(5)]],
      article: ['',]
    });
    /*
    this.newIdeaForm.setValue({
      title: 'new idea poggers',
      tags: ['shroud', 'forsen', 'kek'],
      article: 'omega lul'
    });
    */
  }

  create() {
    if (this.newIdeaForm.valid) {
      this.loading = true;
      this.ideaService.createOrUpdate(this.newIdeaForm.value)
        .finally(() => this.loading = false)
        .subscribe(
          result => {
            console.log(result);
            this.router.navigate(['/']);
          },
          err => {
            console.log(err);
          })
    }
  }
}
