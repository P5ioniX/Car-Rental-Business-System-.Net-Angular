import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserServise } from './services/user.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  showBasicNavBar : boolean = true ;
  userNavBar : boolean = false ;
  employeeNavBar : boolean = false ;
  adminNavBar : boolean = false ;

  constructor(private _http : HttpClient , private _usrService :UserServise ,private _router :Router){
    this.hideNavBarIfLoggedIn();
    this.showRelativeNavBar();
  }

  hideNavBarIfLoggedIn()
  {
    let localTkn = localStorage.getItem('usrTkn');
    let localRole = localStorage.getItem('Role');
    
    
    if(localTkn){   
      if (localRole === null) {
        this.showBasicNavBar = true;
      }
      else{
        this.showBasicNavBar = false;
      }
      
    }


  }
  showRelativeNavBar()
  {
    console.log(localStorage.getItem('usrTkn'));
    console.log(localStorage.getItem('Role'));
    
    this.setRelativeNavbarVeriable(localStorage.getItem('usrTkn'),localStorage.getItem('Role'));

  }

  setRelativeNavbarVeriable(localTkn:string,localRole:string){
    if(localTkn !==null){      
       
      if (localRole === null) { this.showBasicNavBar=true; }
       
      if (localRole === "User") { this.userNavBar=true; }

      if (localRole === "Employee") { this.employeeNavBar=true; }

      if (localRole === "Manager") { this.adminNavBar=true; }
   }
   else{
     this.showBasicNavBar = true; 
     this.employeeNavBar=false;
     this.adminNavBar=false;
     this.userNavBar=false;
   }
   
  };

  title = 'app';
}
