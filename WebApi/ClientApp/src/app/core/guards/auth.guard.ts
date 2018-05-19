import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { JwtTokenService } from '../services/jwt-token.service';

@Injectable()
export class AuthGuard implements CanActivate {
  
  constructor(private router: Router,
              private tokenService: JwtTokenService) { }
 
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
      if (!this.tokenService.isTokenExpired()) {
          return true;
      }

      this.router.navigate(['auth', 'signin'], { queryParams: { returnUrl: state.url }});
      return false;
  }
}
