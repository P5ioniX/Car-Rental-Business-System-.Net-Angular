import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { UserServise } from '../services/user.service';


@Injectable()
export class AdminRoleGuard implements CanActivate {

  customerClaim: string;

  constructor(private _router: Router, private _usrService: UserServise) {

  }


  canActivate() {


    if (localStorage.getItem('Role')===null) 
    {
      localStorage.setItem('usrTkn',null);
    }

    return this.checkStatus(localStorage.getItem('Role'));
  }

  checkStatus(claim: string): boolean {
    //Set token to null if no role is present
    if (localStorage.getItem('Role') === null) {
      localStorage.setItem('usrTkn', null);
    }
    if (claim === "Manager") {
      return true;
    }
    else {
      return false;
    }
  }
}


/*





*/