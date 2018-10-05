import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';


import { HttpErrorResponse } from '@angular/common/http';

import { Router, ActivatedRoute } from '@angular/router';
import { UserServise } from '../../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { LoginClass } from '../../../classes-interfaces/login-class';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginUser: LoginClass = new LoginClass();
  hasLoginError: boolean = false;
  returnUrl: string;
  role: string;
  constructor(private _authService: AuthService, private _router: Router, private _route: ActivatedRoute, private _toastr: ToastrService) { }

  ngOnInit() 
  { 
    this.RoleRedirect(this.role);
  }

  OnSubmit(_lgnFrm: NgForm) {
    this.loginUser = _lgnFrm.value;
    this._authService.usrAuth(this.loginUser).subscribe((data: any) => {
      localStorage.setItem('usrTkn', data.access_token);

      this._authService.getUsrClaims().subscribe(data => {
        localStorage.setItem('Role', data.Role);
        this.role = data.Role;
        window.location.reload();
        this.RoleRedirect(this.role);
      })


    },
      (err: HttpErrorResponse) => {
        this._toastr.error("UserName Or Password InValid", "Error");
        this.hasLoginError = true;
      });

  }

  RoleRedirect(role: string) {
    console.log("Login Component"); 
    switch (role) {
      case "User":
      this._router.navigate(['vehicles']);
      //console.log("User");
        break;
      case "Employee":
      this._router.navigate(['return_rentals']);
      //console.log("Employee");
        break;
      case "Manager":
      this._router.navigate(['panel']);
      //console.log("Manager");
        break;
      default:
        this._router.navigate(['login']);
        break;
    }
  }

  hide = true;
}
