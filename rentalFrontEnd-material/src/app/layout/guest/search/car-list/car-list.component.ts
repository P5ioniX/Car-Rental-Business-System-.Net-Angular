import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import { SearchBarService } from '../../../../services/search-bar.service';
import { IVehicle } from '../../../../classes-interfaces/IVehicle.interface';

@Component({
  selector: 'car-list',
  templateUrl: './car-list.component.html',
  styleUrls: ['./car-list.component.css']
})

export class CarListComponent implements OnInit {

  _vehiclesArray: IVehicle[];
  _vehicleSearchArray: IVehicle[] = undefined;
  _vehicleModel : IVehicle;
  _text:string;

  constructor(private _vehicleSrvs: SearchBarService) { }


  ngOnInit() {
    this.getAll();
  }

  //Get All Vehicles For The Table
  getAll(){
    this._vehicleSrvs.getVehicles().subscribe(
      (success) => 
      { 
        let vehicles = JSON.parse(JSON.stringify(success));
        this._vehiclesArray = vehicles;
      }  
      ,(error) => {}
    );
  }



  //Get all of the makes in returned vehicles for the dropdown list
  findAllMakes(): string[] {
    if (this._vehiclesArray !== undefined) {
      let makes = [];
      this._vehiclesArray.forEach(vehicle => {
        if (!makes.includes(vehicle.Make)) { makes.push(vehicle.Make) }
      });
      return makes;
    }
  }

  //Get all of the models in returned vehicles for the dropdown list
  findModels(make?: string): string[] {
    if (this._vehiclesArray !== undefined) {
      let models = [];
      this._vehiclesArray.forEach(vehicle => {
        if (!models.includes(vehicle.Model)) { models.push(vehicle.Model) }
      });
      return models;
    }
  }

  //Get all of the gear types in returned vehicles for the dropdown list
  findGearTypes(): string[] {
    if (this._vehiclesArray !== undefined) {
      let gearTypes = [];
      this._vehiclesArray.forEach(vehicle => {
        if (!gearTypes.includes(vehicle.Gear_Type)) { gearTypes.push(vehicle.Gear_Type) }
      });
      return gearTypes;
    }

  }

  rerenderTable(data:IVehicle[]){
    this._vehicleSearchArray = data;
  }
}

export const vehicleLst = {
  vehicleList : this._vehiclesArray
};
