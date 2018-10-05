import { Component, OnInit } from '@angular/core';
import { RentalService } from '../../../services/rental.service';
import { IRental } from '../../../classes-interfaces/IRental';


@Component({
  selector: 'app-customer-rentals',
  templateUrl: './customer-rentals.component.html',
  styleUrls: ['./customer-rentals.component.css']
})
export class CustomerRentalsComponent implements OnInit {

  constructor(private _rentals: RentalService) { }

  rentals:IRental[];
  returnedRentals:IRental[] = [];
  allRentals:IRental[] = [];
  noneRented: boolean;

  ngOnInit() {
    this.getMyRentals();

  }

  getMyRentals(){
    this._rentals.getRentals().subscribe(
      (success) => {
        if (success!==null) {
          this.sortRentalsInVeriables(JSON.parse(JSON.stringify(success)));
        }
        else{ this.noneRented=true; }
      })
        ,(err)=>{};
  }

  sortRentalsInVeriables(rentals : any){

    if (rentals !== null) {
      this.noneRented = false;
      for (const element of rentals) {
        if (element.Returned_Date!==null) {
          this.returnedRentals.push(element);
          
        }
        else 
        {
          this.allRentals.push(element);
        }
      }
    }
  }

}
