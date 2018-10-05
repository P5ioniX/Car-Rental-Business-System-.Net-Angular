import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs/Observable";

import { Subscription } from 'rxjs';
import "rxjs/operator/map";
import { ActivatedRoute } from '@angular/router';
import { log } from 'util';
import { NgForm, FormGroup } from '@angular/forms';
import { SearchBarService } from '../../../../../services/search-bar.service';
import { IVehicle } from '../../../../../classes-interfaces/IVehicle.interface';
import { IRental } from '../../../../../classes-interfaces/IRental';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})

export class SearchBarComponent implements OnInit{


  @Input()
  vehicleMakes: string[];
  @Input()
  vehicleModels: string[];
  @Input()
  vehicleGearTypes: string[];
  @Input()
  VehicleModels: string[];
  
  @Output() onValueChange = new EventEmitter<any>();

  _loading : boolean = false;

  year: number = (new Date()).getFullYear();
  _vehiclesArray:any;

  constructor(private _vehicleSrvs:SearchBarService,private _ngxToastR:ToastrService) {
    
   }

  ngOnInit() {
    this.populateYears();
  }

  //Populate The Models DropDown Menu In The Component
  rePopulateModels(event) {
    let models = [];

    this.VehicleModels.forEach(vehicle => {
      if (event.value) 
      {
        if (vehicle["Make"] === event.value) {
          if (!models.includes(vehicle["Model"])) {
            models.push(vehicle["Model"])
          }
        }
      } 
      else 
      {
        if (!models.includes(vehicle["Model"])) {
          models.push(vehicle["Model"])
        }
      }
      

      this.vehicleModels = models; });


  }

  //Iterate Through The Last 7 Years And Push To A View Bound Array For Search DropDown List
  populateYears() {
    for (let index = 0; index < 6; index++) {
      this.yearlist.push(this.year);
      this.year--;
    }
  }

  //A Function To Search By A Specific Query Or Get All Of The Vehicles & Raise A Proper Event
  onFieldsChanged(search :NgForm){

    let vehicle : IVehicle = 
    {
      Make:search.value.Make,
      Model:search.value.Model,
      Year:search.value.Year,
      Gear_Type:search.value.GearType,
      Daily_Rate:null,
      Mileage:null,
      Penalty_Rate:null,
      Vehicle_ID:null
    };
    
    let dates   : IRental = {Vehicle_ID:null,Start_Date:search.value.start,Return_Date:search.value.end,Returned_Date:null};

    //console.log(vehicle);
    //console.log(dates);
    
    
    let vehicleValid = this.checkVehicle(vehicle);
    let datesValid = this.checkDates(dates,search.value);
    //console.log(!vehicleValid);
    //console.log(!datesValid);
    //console.log(search.value.text == "");
    
    
    
    if (!vehicleValid && !datesValid && (search.value.text == "")) 
    {
      this.getAll();
    } 
    else
    {

        let text:string = search.value.text;
        this.getByQuery(text,vehicle,dates);
    }   
  }


  //Get Vehicles From The BackEnd Based On A Query From A Search Form And Raise An Event
  getByQuery(text:string,model:IVehicle,desiredDates:any):any{

    if (desiredDates.Start_Date!==null && desiredDates.Return_Date !==null) 
    {
      this._vehicleSrvs.getQueryVehicles(text,model,desiredDates).subscribe(data =>{  
        //console.log("returned from server --->" + JSON.stringify(data)); 
         this.onValueChange.emit(data);
    
      });
    } 
    else 
    {
      this._vehicleSrvs.getQueryVehicles(text,model).subscribe(data =>{  
       // console.log("returned from server --->" + JSON.stringify(data)); 
         this.onValueChange.emit(data);
    
      });
    }
}

    //Get All Vehicles From The BackEnd And Raise An Event
    getAll():any{
      return this._vehicleSrvs.getVehicles().subscribe(data=>{this.onValueChange.emit(JSON.parse(JSON.stringify(data)));});
    }


    checkVehicle(car: any): boolean {
      if (
         car.Make===(""||undefined||null) 
      && car.Model===(""||undefined||null) 
      && car.Year===(""||undefined||null) 
      && car.Gear_Type === (""||undefined||null) 
      && car.Daily_Rate===null 
      && car.Mileage ===null 
      && car.Penalty_Rate===null 
      && car.Vehicle_ID===null) 
      {
        return false;
      }
      return true;
    }

    checkDates(desiredDates:any,searchForm:NgForm):boolean
    {
      
      if ((desiredDates.Start_Date ==(null || "" || undefined) )||(desiredDates.Return_Date == (null || "" || undefined) ) || desiredDates == (null || undefined))
      {
        if ((desiredDates.Start_Date == (null || "" || undefined) )&&(desiredDates.Return_Date == (null || "" || undefined) )) {
          return true;
        } 
        else 
        {
          this._ngxToastR.info("Please fill In Both Dates When Searching By Date");
          return false;
        }
      }
      else
      {
        if (!this.checkIfDatesAreValid(desiredDates.Start_Date,desiredDates.Return_Date)) {
          return false;
        }
        return true;
      }

    }

    clearDates(form:NgForm)
    {
      //console.log(form);
      form.controls.start.setValue(null);
      form.controls.end.setValue(null);
    }

    checkIfDatesAreValid(start,end) :boolean
    {

          let timeMilliseconds = new Date(end).getTime() - new Date(start).getTime();
          let oneDay=1000*60*60*24;
          let daysBetweenDates = Math.floor(timeMilliseconds/oneDay);
          if (daysBetweenDates<0) {
            this._ngxToastR.error("Please Enter A Valid Range");
            return false;          
          }

    }

  yearlist = [];


}
