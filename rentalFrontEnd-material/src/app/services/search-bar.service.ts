import { Injectable } from '@angular/core';
import { Http , Response, Headers } from '@angular/http';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs/Observable";

import "rxjs/Rx";
import { Subscription } from 'rxjs/Rx';
import { IUpdateVehicle } from '../classes-interfaces/IUpdateVehicle.interface';
import { IVehicle } from '../classes-interfaces/IVehicle.interface';
import { IVehicleState } from '../classes-interfaces/IVehicleState.interface';
import { IRental } from '../classes-interfaces/IRental';


@Injectable()
export class SearchBarService {

  _baseUrl : string = "http://localhost:64403/";
  _headers = new HttpHeaders().set('Content-Type', 'application/json').set('No-Auth','True');

  constructor(private _http:HttpClient) { }


/*-----------------Fetch Methods Start--------------------*/
  getVehicles()
  {
    return this._http.get(this._baseUrl+"api/Vehicles/GetAll",{headers:this._headers});
  }

  getVehicleByID(id:number)
  {
    return this._http.get(this._baseUrl+`api/Vehicles/GetByID/${id}`,{headers:this._headers});
  } 

  getVehicleWithStateByID(id:number)
  {
    return this._http.get(this._baseUrl+`api/Vehicles/GetWithStateByID/${id}`);
  } 

  getQueryVehicles(_query:string,_queryVehicle:IVehicle,_dates?:IRental)
  {
    let viewModel = {_queryVehicle,_dates};
    return this._http.post(this._baseUrl+`api/Vehicles/GetByQuery/?_query=${_query}`,viewModel,{headers:this._headers});
  }

  getAllMakes()
  {
    return this._http.get(this._baseUrl+"api/Vehicles/GetMakes");
  }

  getMakeModels(make : string)
  {
    let params = new HttpParams().set('make', make);
    return this._http.get(this._baseUrl+"api/Vehicles/GetModels",{params:params});
  }

  getVehicleTypesForMake(make : string)
  {
    let params = new HttpParams().set('make', make);
    return this._http.get(this._baseUrl+"api/Vehicles/GetVehicleTypesForMake",{params:params});
  }
/*-----------------Fetch Methods End----------------------*/




/*-----------------Add Methods Start--------------------*/
  addMake(make : string)
  {
    let params = new HttpParams().set('make', make);
    return this._http.get(this._baseUrl+"api/Vehicles/AddMake",{params:params});
  }

  addModel(make : string , model : string)
  {
    let params = new HttpParams().set('model', model);
    return this._http.get(this._baseUrl+`api/Vehicles/AddModel?make=${make}&model=${model}`);
  }

  addVehicleType(_carType : IVehicle)
  {
    return this._http.post(this._baseUrl+"api/Vehicles/AddVehicleType" , _carType);
  }

  TryAddVehicle(vehicleModel : IVehicle)
  {
    return this._http.post(this._baseUrl+"api/Vehicles/AddVehicle" , vehicleModel);
  }
/*-----------------Add Methods End----------------------*/



/*-----------------Update Methods Start--------------------*/
patchUpdateVehicle(_vehicle : IUpdateVehicle)
{
  return this._http.patch(this._baseUrl+"api/Vehicles/UpdateVehicleById" , _vehicle);
}

patchUpdateVehicleStatus(state : IVehicleState)
{
  console.log(state);
  return this._http.patch(this._baseUrl+"api/Vehicles/UpdateVehicleStatus", state);
}
/*-----------------Update Methods End----------------------*/



/*-----------------Delete Methods Start--------------------*/
  deleteMake(make : string)
  {
    let params = new HttpParams().set('make', make);
    return this._http.delete(this._baseUrl+"api/Vehicles/TryDeleteMake",{params:params});
  }


  deleteModel(make : string , model : string)
  {
    let params = new HttpParams().set('make', make).append('model', model);
    return this._http.delete(this._baseUrl+"api/Vehicles/TryDeleteModel",{params:params});
  }


  deleteVehicleType(_carType : IVehicle)
  {
    return this._http.delete(this._baseUrl+"api/Vehicles/TryDeleteType",{ params:new HttpParams().set('type',JSON.stringify(_carType))});
  }
/*-----------------Delete Methods End----------------------*/



}





