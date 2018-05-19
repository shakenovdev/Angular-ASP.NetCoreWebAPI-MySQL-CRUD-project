import { Injectable } from '@angular/core';
import * as jwt_decode from 'jwt-decode';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { SignedUser } from '../models';

@Injectable()
export class JwtTokenService {

  getToken(): string {
    return localStorage.getItem('JWT_TOKEN');
  }

  // Token setting is in accountService, cannot use it here due to `curcilar dependency`
  // because has to save token and signedUser => update subject subscriptions about signedUser
  // Probably, it is the weak of architecture

  destroyToken() {
    localStorage.removeItem('JWT_TOKEN');
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    const date = this.getTokenExpirationDate();
    if (date === undefined) {
      this.destroyToken();
      return true;
    }

    const isExpired = !(date.valueOf() > new Date().valueOf());
    if (isExpired) this.destroyToken();
    return isExpired;
  }

  getTokenExpirationDate() {
    const token = this.getToken();
    if (!token) return null;

    const decoded = jwt_decode(token);
    if (decoded['exp'] === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded['exp']);
    return date;
  }
  
}
