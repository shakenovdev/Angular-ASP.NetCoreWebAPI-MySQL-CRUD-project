import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import * as jwt_decode from 'jwt-decode';
import { JwtTokenService } from './jwt-token.service';
import { 
  SignedUser, 
  UserSettings
} from '../models';

@Injectable()
export class AccountService {

  private readonly apiUrl = 'api/account';
  private subject = new Subject<any>();

  constructor(private http: HttpClient,
              private tokenService: JwtTokenService) { }
    
  currentUser = this.getSignedUser();
  
  signUp(user) {
    return this.http.post<any>(`${this.apiUrl}/signup`, user);
  }

  signIn(user) {
    return this.http.post<any>(`${this.apiUrl}/signin`, user);
  }

  signOut() {
    this.tokenService.destroyToken();
    localStorage.removeItem('SIGNED_USER');
    this.subject.next();
  }

  saveSignedUser(token: string) {
    localStorage.setItem('JWT_TOKEN', token);
    var decodedToken: any = jwt_decode(token);

    var signedUser: SignedUser = {
      name: decodedToken.name,
      avatarUrl: decodedToken.avatarUrl
    }; 
    localStorage.setItem('SIGNED_USER', JSON.stringify(signedUser));
    this.subject.next(signedUser);
  }

  getSignedUser(): SignedUser {
    if (this.tokenService.isTokenExpired()) {
      localStorage.removeItem('SIGNED_USER');
      return null;
    }
    
    var signedUser = localStorage.getItem('SIGNED_USER');
    return JSON.parse(signedUser);
  }

  changeSignedUser(userSettings: UserSettings) {
    var updatedSignedUser: SignedUser = {
      name: userSettings.userName,
      avatarUrl: userSettings.avatarURL
    };

    localStorage.setItem('SIGNED_USER', JSON.stringify(updatedSignedUser));
    this.subject.next(updatedSignedUser);
  }

  changedSignedUser(): Observable<any> {
    return this.subject.asObservable();
  }

  extendToken() {
    const token = this.tokenService.getToken();
    if (!token) return false;

    const date = this.tokenService.getTokenExpirationDate();
    if (date === undefined) {
      this.tokenService.destroyToken();
      return false;
    }

    if (this.daysTo(date) < 5) {
      this.http.post<any>(`${this.apiUrl}/extendToken`, null).subscribe(
        data => {
          this.saveSignedUser(data);
          return true;
        },
        err => { 
          return false; 
        }
      )
    }
  }

  

  private daysTo(date) {
    var now = new Date().valueOf();
    date = date.valueOf();
    return Math.round((date-now)/(1000*60*60*24));
  }

}
