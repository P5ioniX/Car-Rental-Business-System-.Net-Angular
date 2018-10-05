import { Component, OnInit, Inject, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchBarService } from '../../../../services/search-bar.service';

import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { MatDialog , MatDialogRef , MAT_DIALOG_DATA } from '@angular/material';
import { IUpdateVehicle } from '../../../../classes-interfaces/IUpdateVehicle.interface';
import { IVehicle } from '../../../../classes-interfaces/IVehicle.interface';
import { IVehicleState } from '../../../../classes-interfaces/IVehicleState.interface';

@Component({
  selector: 'vehicle-editor',
  templateUrl: './vehicle-editor.component.html',
  styleUrls: ['./vehicle-editor.component.css']
})
export class VehicleEditorComponent implements OnInit {

  vehicle : any ;
  updateVehicle:IVehicle = null ;

  makeVehicleTypes : IVehicle[];
  vehicleCreateType : IVehicle;


  makeString : string ;
  modelString : string ;

  chosenMake : string ;
  chosenModel : string ;

  makes:any ;
  models:string[] ;

  constructor(private _vhcle : SearchBarService,private _ngxToastR : ToastrService,private _router : Router,public dialog: MatDialog) { }

  openDialog(vehicleType : IVehicle): void {
    let dialogRef = this.dialog.open(AddVehicleDialog, {
      width: '250px',
      data: vehicleType 
    });

    dialogRef.afterClosed().subscribe(result => {

    });
  }


  ngOnInit() {
    this.getValuesForMakesBox();
  }

  getValuesForMakesBox()
  {
    this._vhcle.getAllMakes().subscribe(
      (success)=>{this.makes=success}
      ,(error)=>{ }
    );
  }

  getValuesForModelsBox(event)
  {
    this.chosenModel=null;
    if (event.value!==undefined) 
    {
      this._vhcle.getMakeModels(event.value).subscribe(
        (success)=>
        {
          this.models=<string[]>success;
          if (this.models.length==0) 
          {
            this.models=undefined;
          }
        }
        ,(error)=>{}
      );
    }
    else
    {
      this.models=undefined;
    }
    
  }

  getVehicleForEdit(vehicleID : number)
  {

    this._vhcle.getVehicleWithStateByID(vehicleID).subscribe(
      (success)=>
      {
        this.vehicle = success;
        this.updateVehicle = this.vehicle.m_Item1;
      },
      (error)=>
      { }
    );

  }

  patchUpdatedVehicle(updateForm : NgForm ,vehicleID :number ,state? : IVehicleState)
  {
    let updateVehicle : IUpdateVehicle = {Vehicle_ID:vehicleID,Mileage:updateForm.value.mileage};
    console.log(updateVehicle);
    
    let vehicleState : IVehicleState = {Available:updateForm.value.available,Rentable:updateForm.value.rentable,Vehicle_ID:vehicleID};

    this._vhcle.patchUpdateVehicle(updateVehicle).subscribe(
      (success)=>{
        this._vhcle.patchUpdateVehicleStatus(vehicleState).subscribe(
          (success)=>{
            this._ngxToastR.success(JSON.stringify(success));
          },
          (error)=>{this._ngxToastR.error(JSON.stringify(error.error))}
        )
      },
      (error)=>{this._ngxToastR.error(JSON.stringify(error.error))}
    );
  }

  nullifyValues()
  {
    let clear = ()=>{this.chosenMake = undefined; this.models=undefined; this.makeString=undefined;};
    clear();
  }

  AddMake(makeString : string)
  {
    this._vhcle.addMake(makeString).subscribe(
      (success)=>
      {
        this.getValuesForMakesBox();
        this._ngxToastR.success(success.toString(),"Congratulations");
      }
      ,(error)=>
      {
        this._ngxToastR.error(error.error,"Error");
      }
    );
  }

  TryDeleteMake(makeString : string)
  {
    this._vhcle.deleteMake(makeString).subscribe(
      (success)=>
      {
        this.getValuesForMakesBox();
        this.nullifyValues();
        this._ngxToastR.success(success.toString(),"Congratulations");
        
      }
      ,(error)=>
      {
        this._ngxToastR.error(error.error,"Error");
      }
    );
  }

  AddModel(makeString : string , modelString : string)
  {
    this._vhcle.addModel(makeString,modelString).subscribe(
      (success)=>
      {

        this._vhcle.getMakeModels(makeString).subscribe(
          (success)=>
          {
            this.models=<string[]>success;
            if (this.models.length==0) 
            {
              this.models=undefined;
            }
          }
          ,(error)=>{}
        );


        this._ngxToastR.success(success.toString(),"Congratulations");
      }
      ,(error)=>
      {
        this._ngxToastR.error(error.error,"Error");
      }
    );
  }  

  TryDeleteModel(makeString : string ,modelString : string)
  {
    this._vhcle.deleteModel(makeString,modelString).subscribe(
      (success)=>
      {
        let clear = ()=>{this.chosenModel=undefined;};
        clear();
        this.getValuesForModelsBox(makeString);
        this._ngxToastR.success(success.toString(),"Congratulations");
        
      }
      ,(error)=>
      {
        this._ngxToastR.error(error.error,"Error");
      }
    );

  }

  GetAllMakeVehicleTypes(event)
  { 
    this.makeVehicleTypes = null;

    if (event.value != undefined) 
    {
      this._vhcle.getVehicleTypesForMake(event.value).subscribe(
        (success)=>{
          let responce = JSON.parse(JSON.stringify(success));
          if (responce[0]==="Error") 
          {
            this._ngxToastR.info(responce[1]);
          } 
          else 
          {
            this.makeVehicleTypes=JSON.parse(JSON.stringify(success));
          }
        },
        (error)=>{
          this._ngxToastR.error(error.error,"Error");
        }
      );
    }
    
  }

  GetAllMakeVehicleTypesEventLess(make : string)
  {
    this.makeVehicleTypes = null;

    if (make != undefined) 
    {
      this._vhcle.getVehicleTypesForMake(make).subscribe(
        (success)=>{
          let responce = JSON.parse(JSON.stringify(success));
          if (responce[0]==="Error") 
          {
            this._ngxToastR.info(responce[1]);
          } 
          else 
          {
            this.makeVehicleTypes=JSON.parse(JSON.stringify(success));
          }
        },
        (error)=>{
          this._ngxToastR.error(error.error,"Error");
        }
      );
    }
  }

  AddType(typeForm : NgForm , makeString : string , modelString : string)
  {
  let carType : IVehicle = 
    {
      Make:makeString,
      Model:modelString,
      Gear_Type:typeForm.value.gear,
      Year:typeForm.value.year,
      Daily_Rate:typeForm.value.dailyrate,
      Penalty_Rate:typeForm.value.penaltyrate,
      Mileage:null,
      Vehicle_ID:null,
      Available:true
    };  

    this._vhcle.addVehicleType(carType).subscribe(
      (success)=>{
        this._ngxToastR.success(`${carType.Make + " " + carType.Model} Added`,"Congratulations");
        this.GetAllMakeVehicleTypesEventLess(makeString);
      },
      (error)=>{
        this._ngxToastR.error(error.error,"Error");
      }
    );
  }
  
  deleteType(type : IVehicle)
  {
    this._vhcle.deleteVehicleType(type).subscribe(
      (success)=>{
        this._ngxToastR.success(`${type.Make + " " + type.Model} Deleted`,"Congratulations");
        this.GetAllMakeVehicleTypesEventLess(type.Make);
      },
      (error)=>{
        this._ngxToastR.error(error.error,"Error");
      }
    );
  }
}

/*----------------------------------------------------------------------------------------------------------------------------------*/

@Component({
  selector: 'Add-Vehicle-Dialog',
  template: `
  
  <h2 mat-dialog-title>{{data.Make}}</h2>
  <h4 mat-dialog-title>{{data.Model}}</h4>

  <mat-divider></mat-divider>

  <mat-dialog-content>

  <mat-form-field>
  <input matInput #carNumber placeholder="Vehicle Number" required>
  </mat-form-field>

  <mat-form-field>
  <input matInput #mileage placeholder="Mileage" required>
  </mat-form-field>

  </mat-dialog-content>

  <mat-divider></mat-divider>

  <mat-dialog-actions>
  <button mat-raised-button (click)="onAdd(data,carNumber.value,mileage.value)" color="primary">Add</button>
  <button mat-raised-button (click)="onCancel()" color="primary">Cancel</button>
  </mat-dialog-actions>
  
  
  `,
})
export class AddVehicleDialog {

  constructor(
    public dialogRef: MatDialogRef<AddVehicleDialog>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: IVehicle,private _ngxToastR : ToastrService , private _vhcle : SearchBarService) { }

  onAdd(vehicleType : IVehicle , number : any , mileage : any)
  {
    if (vehicleType != null && number != "" && mileage != "") 
    {
      let vehicleToAdd : IVehicle = vehicleType ;
      vehicleToAdd.Vehicle_ID = number;
      vehicleToAdd.Mileage = mileage;

      this._vhcle.TryAddVehicle(vehicleToAdd).subscribe(
        (success)=>{
          this.dialogRef.close();
          this._ngxToastR.success(`${vehicleToAdd.Make + " " + vehicleToAdd.Model} Added`,"Congratulations");
        },
        (error)=>{
          this._ngxToastR.error(error.error,"Error");
        }
      );
      

      console.log(vehicleToAdd);
    } 
    else 
    {
      this._ngxToastR.info("Please Fill All Values");
    }

    
  }

  onCancel(): void 
  {
    this.dialogRef.close();
  }

}