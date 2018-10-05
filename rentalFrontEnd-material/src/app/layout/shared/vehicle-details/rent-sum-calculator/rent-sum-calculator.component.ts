import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'rent-sum-calculator',
  templateUrl: './rent-sum-calculator.component.html',
  styleUrls: ['./rent-sum-calculator.component.css']
})
export class RentSumCalculatorComponent implements OnInit {

  startDate : Date;
  endDate : Date;
  numberOfDays : number;
  @Input() price : number;
  selectedRentalPrice : string;
  wrongCalculation : boolean = false;

  constructor(private _toastr: ToastrService) { }

  ngOnInit() {
  }

  calculatePrice(){
      if (this.startDate!==undefined && this.endDate!==undefined) {
        let timeMilliseconds = new Date(this.endDate).getTime() - new Date(this.startDate).getTime();
        let oneDay=1000*60*60*24;
        let daysBetweenDates = Math.floor(timeMilliseconds/oneDay);
        if (daysBetweenDates<0) {
          this._toastr.error("Can't calculate this range");
          return;          
        }
        this.selectedRentalPrice = (daysBetweenDates*this.price).toString() + " ₪";
      }
      else{
        this._toastr.error("Values Not Selected Correctly .");
      }
  }

  dayDiff(startdate, enddate) {
    var dayCount = 0;
  
    while(enddate >= startdate) {
      dayCount++;
      startdate.setDate(startdate.getDate() + 1);
    }
  
  return dayCount; 
  }

}
