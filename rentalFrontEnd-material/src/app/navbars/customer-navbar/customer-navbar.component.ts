import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'customer-navbar',
  templateUrl: './customer-navbar.component.html',
  styleUrls: ['./customer-navbar.component.css']
})
export class CustomerNavbarComponent implements OnInit {

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
