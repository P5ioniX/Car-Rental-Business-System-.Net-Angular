import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrls: ['./admin-navbar.component.css']
})
export class AdminNavbarComponent implements OnInit {

  constructor(private _router : Router) { }

  ngOnInit() {
  }

  LogOut()
  {
    localStorage.removeItem('Role');
    localStorage.removeItem('usrTkn');
    this._router.navigate(['/main']);
    window.location.reload(true);
  }

}
