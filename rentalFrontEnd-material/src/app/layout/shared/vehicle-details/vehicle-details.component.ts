import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { vehicleLst } from '../../guest/search/car-list/car-list.component';
import { SearchBarService } from '../../../services/search-bar.service';
import { IVehicle } from '../../../classes-interfaces/IVehicle.interface';
import { forEach } from '@angular/router/src/utils/collection';


@Component({
  selector: 'app-vehicle-details',
  templateUrl: './vehicle-details.component.html',
  styleUrls: ['./vehicle-details.component.css']
})
export class VehicleDetailsComponent implements OnInit {


  constructor(private _route : ActivatedRoute , private _vhclservice : SearchBarService) { }

  vehicle : any;
  
  ngOnInit() {

    this._vhclservice.getVehicleByID(this._route.snapshot.params['id']).subscribe(
      data => {

        this.setLocalStorage(data);

        this.vehicle = data;

      }), err => console.error(err); /* () => console.log('done getting vehicle') */
  }

  setLocalStorage(car : any): any 
  {
    
    let lclStorage  = JSON.parse(localStorage.getItem('vehiclesViewed'));
    let count = (JSON.stringify(lclStorage).match(/Make/g) || []).length;
    let oneCarArr : any[] = [JSON.stringify(car)];
    let carArr : any[] = lclStorage || [];

    if (count === 0) 
    {
      localStorage.setItem('vehiclesViewed',JSON.stringify(oneCarArr));
    } 
    else 
    {
        if (!carArr.includes(JSON.stringify(car))) {
          if (count>=5) 
          {
            carArr.shift();
            carArr.push(JSON.stringify(car));
            localStorage.setItem('vehiclesViewed',JSON.stringify(carArr));
          }
          else if(count<5)
          {
            carArr.push(JSON.stringify(car));
            localStorage.setItem('vehiclesViewed',JSON.stringify(carArr));
          }
        }
    }
 
  }

}
