import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private _router : Router){}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) : boolean {

 
      


      if(localStorage.getItem('usrTkn')!==null){
        console.log("AuthGuard If");
         return true; 
        }
      else { 
        console.log("AuthGuard else");
        this._router.navigate(['/login']);  
        window.location.reload(true);  
        return false;
      }
  }
}
