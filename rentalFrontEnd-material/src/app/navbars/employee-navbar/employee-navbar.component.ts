import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'employee-navbar',
  templateUrl: './employee-navbar.component.html',
  styleUrls: ['./employee-navbar.component.css']
})
export class EmployeeNavbarComponent implements OnInit {

  constructor(private _router : Router) { }

  ngOnInit() {
  }


  LogOut(){
    localStorage.removeItem('usrTkn');
    localStorage.removeItem('Role');
    this._router.navigate(['/main']);
    window.location.reload(true); 
  }
}
