import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch';
import { Router } from '@angular/router';
import { UserServise } from '../services/user.service';

@Injectable()
export class FirstHttpInterceptor implements HttpInterceptor{

    constructor(private _router : Router , private _usrService : UserServise){}

    intercept(req:HttpRequest<any>,next:HttpHandler) : Observable<HttpEvent<any>>
    {
        
        if (req.headers.get('No-Auth') === "True" || req.headers.get('No-Auth')) 
        {
         return next.handle(req.clone());   
        } 
        else
        {
            let localStorageToken = localStorage.getItem('usrTkn');

            if (localStorageToken !== null) 
            {
                /* Add Token From Local Storage To Header */
                const clonedRequest = req.clone({
                    headers:req.headers.set('Authorization','Bearer '+ localStorageToken)
                });
                return next.handle(clonedRequest)
                .do(
                    (success)=>{ 
                        /*Do Nothing*/
                        //console.log("Interceptor - success"); 
                        return next.handle(req.clone());
                    },
                    (error)=>
                    {
                        /* Token Expired ? ---> Redirect To Login */
                        if (error.status === 401) {
                            //console.log("Interceptor 401 Responce Error");
                            localStorage.removeItem('usrTkn');
                            localStorage.removeItem('Role'); 
                            window.location.reload(true);
                            this._router.navigate(['login']);
                        }
                    }
                )
            }
            else
            { 
                /* No Token Present ---> Redirect To Login */ 
                this._router.navigate(['login']); 
            }

        }

        

    }
}

