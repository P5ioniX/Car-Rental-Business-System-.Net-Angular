import { Injectable } from '@angular/core';
import { LoginClass } from '../classes-interfaces/login-class';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthService {

  constructor(private _httpClient : HttpClient) { }

  readonly rootURL = 'http://localhost:64403/' ;
  
  /* Get Token On Login */
  usrAuth(_usrLgn : LoginClass){
    const usrData : LoginClass = {userName:_usrLgn.userName , passWord:_usrLgn.passWord};
    let lgnData = "UserName="+usrData.userName+"&password="+usrData.passWord+"&grant_type=password";
    let requestHeaders = new HttpHeaders({'Content-Type':'application/x-www-urlencoded','No-Auth':'True'});
    return this._httpClient.post(this.rootURL+'retrievetoken',lgnData,{headers:requestHeaders});
  }

   /* Get User Claims After Recieving A Token */
  getUsrClaims():Observable<any>{
    return this._httpClient.get(this.rootURL+'api/User/GetUserClaims');
  }

}
