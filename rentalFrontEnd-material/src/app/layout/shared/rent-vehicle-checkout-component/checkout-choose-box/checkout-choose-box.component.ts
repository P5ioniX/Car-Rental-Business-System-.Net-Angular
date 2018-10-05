import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { RentalService } from '../../../../services/rental.service';
import { IRental } from '../../../../classes-interfaces/IRental';
import { IVehicle } from '../../../../classes-interfaces/IVehicle.interface';

@Component({
  selector: 'checkout-choose-box',
  templateUrl: './checkout-choose-box.component.html',
  styleUrls: ['./checkout-choose-box.component.css']
})

export class CheckoutChooseBoxComponent implements OnInit {

  @Input() vehicle : IVehicle;

  startDate : Date;
  endDate : Date;

  constructor(private _toastr : ToastrService,private _rentalSrvs:RentalService){}

  ngOnInit() { 
  }

  checkOut(_vehicleID : number)
  {

    let vehicleID = _vehicleID;


    if (this.startDate!==undefined && this.endDate!==undefined) {
      let timeMilliseconds = new Date(this.endDate).getTime() - new Date(this.startDate).getTime();
      let oneDay=1000*60*60*24;
      let daysBetweenDates = Math.floor(timeMilliseconds/oneDay);
      if (daysBetweenDates<0) {
        this._toastr.error("Not A Valid Time Period","Can't Rent");
        return;          
      }
      let rental:IRental = {Start_Date:this.startDate,Return_Date:this.endDate,Vehicle_ID:_vehicleID,Returned_Date:null}; 
      //Post Rental To Server Here :
      this._rentalSrvs.postRental(rental).subscribe(
        success => {

          this._toastr.success(this.endDate.toDateString(),"Dont Forget To Return By : ")},
        error =>{
          this._toastr.error(JSON.stringify(error.error)); 
        }
      );
      
    }
    else{
      this._toastr.error("Values Not Selected Correctly .","Can't Rent");
    }
  }
}
