import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ScopeGuardService { // TO DO - rename to role guard or else figure out how to use scopes

  constructor(public auth: AuthService, public router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {

    const scopes = (route.data as any).expectedRoles;

    if (!this.auth.isAuthenticated() || !this.auth.userHasRoles(scopes)) {
      console.log('access denied');
      this.router.navigate(['']);
      return false;
    }
    return true;
  }
}
