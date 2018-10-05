import { Component, OnInit, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { RentalService } from '../../../../services/rental.service';
import { IRental } from '../../../../classes-interfaces/IRental';

@Component({
  selector: 'rental-results',
  templateUrl: './rental-results.component.html',
  styleUrls: ['./rental-results.component.css']
})
export class RentalResultsComponent implements OnInit {

  @Input() rental:IRental;

  constructor(private _rentals : RentalService,private _toastr : ToastrService) { }

  ngOnInit() {
  }

  returnRental(rental:IRental,miles:number){
    this._rentals.returnRentalByID(rental.Vehicle_ID,miles).subscribe(
      (success)=>{this._toastr.success("Rental Closed","Success"); this.rental=null;},
      (verbError)=>{this._toastr.error(verbError.error,"Error")}
    ),(error)=>{this._toastr.error("Something wen't WRONG","Error")};
  }

}
