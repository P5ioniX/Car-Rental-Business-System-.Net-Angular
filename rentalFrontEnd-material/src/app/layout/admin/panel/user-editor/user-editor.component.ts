import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserServise } from '../../../../services/user.service';
import { RegisterClass } from '../../../../classes-interfaces/register-class';

@Component({
  selector: 'user-editor',
  templateUrl: './user-editor.component.html',
  styleUrls: ['./user-editor.component.css']
})
export class UserEditorComponent implements OnInit {

  
  user:any;
  updateUser:any;
  date:any;
  selectedDropBoxValue:string;

  
  
  constructor(private _usrService : UserServise , private _ngxToastr:ToastrService) { }

  ngOnInit(){}

  dateEventValueChange(event:any)
  {
    this.updateUser.dateOfBirth = event.value;
  }

  getUserForEdit(Id : number)
  {
    this._usrService.getUserById(Id).subscribe(
      (success)=>{
      this.user = success;
      this.updateUser = this.user; 
      this.date = new FormControl(new Date(this.user.dateOfBirth).toISOString());
      this.selectedDropBoxValue = this.user.sex;
    }
      ,(error)=>{
        this.updateUser = null;
        this._ngxToastr.error("No User Found");
      }
    );
  }


  patchUpdatedUser(_updateForm : NgForm, _Id : number)
  {
    let userToUpdate:RegisterClass = 
    {
    Id:_Id,
    passWord:null,
    UserName : _updateForm.value.username,
    fullName : _updateForm.value.fullName,
    dateOfBirth : this.updateUser.dateOfBirth,
    sex : _updateForm.value.sex,
    eMail : _updateForm.value.eMail,
    profilePicture : null
    }

    this._usrService.updateUserById(userToUpdate).subscribe(
      (success) =>
      {
        this.updateUser = null;
        _updateForm.resetForm();
        this._ngxToastr.success(userToUpdate.UserName + "'s " + "Details Updated successfully","Congratulations");
      },
      (error)=>
      {
        this._ngxToastr.error("Something wen't terribly wrong","Error");
      }

    );

  }

  deleteUser(_Id : number)
  {
    this._usrService.deleteUserById(_Id).subscribe(
      (success) =>
      {
        this.updateUser = null;
        this._ngxToastr.success(JSON.stringify(success),"Congratulations");
      },
      (error)=>
      {
        this._ngxToastr.error("Something wen't terribly wrong","Error");
      });
  }
}
