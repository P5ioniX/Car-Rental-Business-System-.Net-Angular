import { Component, OnInit } from '@angular/core';
import { IVehicle } from '../../../classes-interfaces/IVehicle.interface';
import { forEach } from '@angular/router/src/utils/collection';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'last-viewed-vehicles',
  templateUrl: './last-viewed-vehicles.component.html',
  styleUrls: ['./last-viewed-vehicles.component.css']
})
export class LastViewedVehiclesComponent implements OnInit {

  local : any[];
  localVehicles : IVehicle[] = [];
  constructor(private _router : Router ,private _route : ActivatedRoute) { }

  ngOnInit() 
  {
    this.GetItemsFromLocalStorage();
  }

  GetItemsFromLocalStorage()
  {
    let lclStorage = localStorage.getItem('vehiclesViewed');
    
    if (lclStorage!==(null || undefined))
     {
       this.local = undefined;
       this.local = JSON.parse(lclStorage);
     };

     this.TransformLocalStorageItem(this.local);

  }

  TransformLocalStorageItem(item:string[])
  {
    if (item) 
    {
      for (let index = 0; index < item.length; index++) {
        const element = item[index];
        this.localVehicles.push(this.transformString(element));

      }
    }
    else
    {
      return null;
    }

    
  }

  transformString(objString:string) : IVehicle
  {
    let trimmedDown = objString.replace(/{/g, '').replace(/}/g, '');
    let splitString = trimmedDown.split(",");
    let filtered = splitString.map((item)=>
    {
      return item.split(":");
    });

    return {
      Make:filtered[0][1].replace(/["']/g, ""),
      Model:filtered[1][1].replace(/["']/g, ""),
      Daily_Rate:parseInt(filtered[2][1]),
      Penalty_Rate:parseInt(filtered[3][1]),
      Year:parseInt(filtered[4][1]),
      Gear_Type:filtered[5][1].replace(/["']/g, ""),
      Vehicle_ID:parseInt(filtered[6][1]),
      Mileage:parseInt(filtered[7][1]),
      Available:true
    };
    
    //let number : number = parseInt(splitString[6].split(":")[1]);
    
    //return number;

  }

  NavigateTo(_id:number)
  {
    this._router.navigateByUrl(`vehicle/${_id}`,{skipLocationChange:false});
    window.location.reload();
  }
}

