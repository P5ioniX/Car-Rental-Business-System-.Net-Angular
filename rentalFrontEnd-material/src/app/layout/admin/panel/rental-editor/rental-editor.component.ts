import { Component, OnInit } from '@angular/core';
import { IRental } from '../../../../classes-interfaces/IRental';
import { RentalService } from '../../../../services/rental.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'rental-editor',
  templateUrl: './rental-editor.component.html',
  styleUrls: ['./rental-editor.component.css']
})
export class RentalEditorComponent implements OnInit {

  userId : number;
  bookedRentals : IRental[]=[];
  openRentals : IRental[]=[];
  closedRentals : IRental[]=[];
  canceledRentals : IRental[]=[];
  lastAction : string ;

  constructor(private _rentals : RentalService, private _toastr : ToastrService) { }

  ngOnInit() {
  }

  getUserRentals(id : number)
  {
    this.bookedRentals = [];
    this.openRentals = [];
    this.closedRentals = [];
    this.canceledRentals = [];

    if (id) 
    {
      this._rentals.getRentalsByUserID(id).subscribe(
        (success)=>
        {
          this.setRentalTypes(success);
          this.lastAction = "GetByID";
        },
        (error)=>
        {
          this._toastr.info(error.error);
        }
      );
    }


  }

  setRentalTypes(rentals:any[])
  {
    let now  = new Date(Date.now()).getUTCDate();
    rentals.forEach(rental=>
      {
        let startDate = new Date(rental.Start_Date).getUTCDate();
        if((startDate>now) && (rental.Returned_Date===null))
        {
          this.bookedRentals.push(rental);
        }
        if(startDate<now && rental.Returned_Date===null)
        {
          this.openRentals.push(rental);
        }
        if (rental.Returned_Date!==null && rental['Total_Price']!==0) 
        {
          this.closedRentals.push(rental);
        }
        if (rental.Returned_Date!==null && rental['Total_Price']===0) 
        {
          this.canceledRentals.push(rental);
        }
      });
  }

  returnRental(vehicleId:number,mileage:number)
  {
    this._rentals.returnRentalByID(vehicleId,mileage).subscribe(
      (success)=>
      {
        this._toastr.success(success,"Excellent");
        this.getUserRentals(this.userId);
      },
      (error)=>
      {
        this._toastr.error(error,"Error");
      }
    );
  }

  cancelRental(rentalId:number)
  {
    this._rentals.cancelRental(rentalId).subscribe(
      (success)=>
      {
        this._toastr.success(success.toString(),"Great");
        this.getUserRentals(this.userId);
      },
      (error)=>
      {
        this._toastr.error(error.error,"Error");
      }
    );
  }

  deleteRental(rentalID:number)
  {
    /*
    m_Item1 :
    m_Item2 : {Rental_ID: 14, User_ID: 12121212, Vehicle_ID: 15246982, Start_Date: "2018-04-18T00:00:00", Return_Date: "2018-04-29T00:00:00", …}
    m_Item3:"Rental Number 14 Deleted"
    */

    this._rentals.deleteRental(rentalID).subscribe(
      (success)=>
      {
        this._toastr.success(success['m_Item2'],"Great");
        this.getUserRentals(this.userId);
      },
      (error)=>
      {
        this._toastr.error(error.error,"Error");
      }
    );
  }

}
