import { Injectable } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

import 'rxjs/add/operator/map'

import { Router } from '@angular/router';
import { RegisterClass } from '../classes-interfaces/register-class';
import { LoginClass } from '../classes-interfaces/login-class';


@Injectable()
export class UserServise {

  readonly rootURL = 'http://localhost:64403/' ;

  constructor(private _httpClient : HttpClient,private _router:Router) { }

  getUserById(_userId : number)
  {
    return this._httpClient.get(this.rootURL + 'api/User/GetUserById?Id=' + _userId);
  }

  updateUserById(model : RegisterClass)
  {
    return this._httpClient.patch(this.rootURL + 'api/User/UpdateUserById?Id=' + model.Id , model );
  }

  registerNewUser(_user : RegisterClass )
  {
    const reqBody:RegisterClass = {
    Id             : _user.Id,     
    fullName       : _user.fullName,  
    eMail          : _user.eMail,  
    UserName       : _user.UserName,  
    passWord       : _user.passWord,  
    dateOfBirth    : _user.dateOfBirth,
    sex            : _user.sex,  
    profilePicture : _user.profilePicture
  }
  let requestHeaders = new HttpHeaders({'No-Auth':'True'});
  return this._httpClient.post(this.rootURL + 'api/User/Register' ,reqBody,{headers:requestHeaders});
  }

  deleteUserById(_Id : number)
  {
    return this._httpClient.delete(this.rootURL + 'api/User/deleteUserById?Id=' + _Id);
  }

}
