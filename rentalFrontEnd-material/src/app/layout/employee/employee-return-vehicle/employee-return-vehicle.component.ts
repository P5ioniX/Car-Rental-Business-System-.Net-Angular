import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { RentalService } from '../../../services/rental.service';
import { IRental } from '../../../classes-interfaces/IRental';

@Component({
  selector: 'app-employee-return-vehicle',
  templateUrl: './employee-return-vehicle.component.html',
  styleUrls: ['./employee-return-vehicle.component.css']
})
export class EmployeeReturnVehicleComponent implements OnInit {

  vehicleID:number;
  resultRental:IRental;

  constructor(private _rentals : RentalService,private _toastr : ToastrService) { }

  ngOnInit() {
  }

  getSelectedVehicleRental(vehicleID : number)
  {
    this._rentals.getOpenRentalByID(vehicleID).subscribe(
      (sucsses)=>{
        if (sucsses!=="No Rental Found") {
          this.resultRental = sucsses
        } else {
          this._toastr.error(JSON.stringify(sucsses),"Oops..")
        }

      },(verbError)=>{this._toastr.error(verbError.error,"Oops..")
      }),(error)=>{this._toastr.error(error.error)};
  }

}
