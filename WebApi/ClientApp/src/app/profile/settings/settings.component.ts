import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { 
  UserSettings,
  AccountService,
  FileService,
  AlertService,
  ProfileService
} from '../../core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit {

  @ViewChild('fileInput') fileInput: any;
  private readonly defaultImage = "person.png";
  currentUser: UserSettings;
  loading = false;

  constructor(private route: ActivatedRoute,
              private accountService: AccountService,
              private fileService: FileService,
              private profileService: ProfileService,
              private alertService: AlertService) { }

  ngOnInit() {
    this.currentUser = this.route.snapshot.data['user'];
  }

  selectImage(event: any) {
    if (event.target.files && event.target.files[0]) {
      // show image preview
      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.currentUser.avatarURL = event.target.result;
      }
      reader.readAsDataURL(event.target.files[0]);
    }
  }

  removeImage() {
    this.fileInput.nativeElement.value="";
    this.currentUser.avatarURL = this.defaultImage;
  }

  update() {
    this.loading = true;
    //upload to server to get avatar url
    if (this.currentUser.avatarURL != this.defaultImage && this.fileInput.nativeElement.files[0]) {
      this.fileService.upload(this.fileInput.nativeElement.files[0]).subscribe(
        data => {
          this.currentUser.avatarURL = data.link;
          this.updateUserSettings();
        },
        err => {
          this.alertService.errors(err.error);
        }
      )
    }
    // update if avatar is null
    else {
      this.updateUserSettings();
    }
    
  }

  private updateUserSettings() {
    this.profileService.updateUserSettings(this.currentUser)
    .finally(() => this.loading = false)
    .subscribe(
      data => {
        this.accountService.changeSignedUser(this.currentUser);
        this.alertService.success("The settings was successfully updated!")
      },
      err => {
        this.alertService.errors(err.error);
      }
    )
  }

}
