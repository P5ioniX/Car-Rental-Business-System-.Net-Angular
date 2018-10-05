import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { IRental } from '../classes-interfaces/IRental';


@Injectable()
export class RentalService {

  readonly rootURL = 'http://localhost:64403/' ;

  constructor(private _http : HttpClient) {
    
   }



  getRentals():Observable<any>{
    return this._http.get(this.rootURL + 'api/Rentals/GetAllRentals');
  }

  getRentalByID(id:number):Observable<any>{
    return this._http.get(this.rootURL + 'api/Rentals/GetRental?vehicleID='+`${id}`);
  }
  
  getOpenRentalByID(id:number):Observable<any>{
    return this._http.get(this.rootURL + 'api/Rentals/GetOpenRental?vehicleID='+`${id}`);
  }

  deleteRental(rentalId:number)
  {
    return this._http.delete(this.rootURL + "api/Rentals/DeleteRental?rentalId="+`${rentalId}`);
  }

  cancelRental(vehicleID:number)
  {
    return this._http.put(this.rootURL + "api/Rentals/CancelRental?vehicleID="+`${vehicleID}`,{vehicleID});
  }

  getRentalsByUserID(userId:number):Observable<any>
  {
    return this._http.get(this.rootURL + "api/Rentals/GetUserRentals?userId="+`${userId}`);
  }

  postRental(_rental:IRental):Observable<any>{
    //let requestHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    let requestHeaders = new HttpHeaders({'Content-Type':'application/x-www-urlencoded','usrTkn':localStorage.getItem('usrTkn')});
    return this._http.post(this.rootURL + 'api/Rentals/Rent',_rental);
  }

 returnRentalByID(id:number,mileage:number):Observable<any>{
    return this._http.put(this.rootURL + "api/Rentals/ReturnRental?id="+`${id}`+"&mileage="+`${mileage}`,{id,mileage});
  }

  

}
