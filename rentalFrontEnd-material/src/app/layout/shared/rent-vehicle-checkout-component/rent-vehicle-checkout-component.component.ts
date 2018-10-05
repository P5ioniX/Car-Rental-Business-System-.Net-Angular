import { Component, OnInit } from '@angular/core';
import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { SearchBarService } from '../../../services/search-bar.service';

@Component({
  selector: 'rent-vehicle-checkout-component',
  templateUrl: './rent-vehicle-checkout-component.component.html',
  styleUrls: ['./rent-vehicle-checkout-component.component.css']
})
export class RentVehicleCheckoutComponentComponent implements OnInit {

  constructor(private _vehicleService : SearchBarService,private _route :ActivatedRoute) { }
 
  vehicle:any;
  
  ngOnInit() {
    var carID = this._route.snapshot.url[1];
    this._vehicleService.getVehicleByID(parseInt(carID.toString())).subscribe(data => {this.vehicle = data});
  }

}
