import { Component, OnInit } from '@angular/core';
import { NgForm, FormBuilder, FormGroup, Validators, NgModel, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserServise } from '../../../services/user.service';
import { RegisterClass } from '../../../classes-interfaces/register-class';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  formHasErrors: string;
  regForm: NgForm;
  registerUser: RegisterClass = new RegisterClass();
  startDate = new Date(2000, 0, 1);


  constructor(private _usrService: UserServise ,
    private _toastr: ToastrService) {
  }

  ngOnInit() {

    this.resetFormFields();
  }

  resetFormFields(_form?: NgForm) {
    if (_form != null) {
      this.registerUser = {
        Id: null,
        fullName: '',
        eMail: '',
        UserName: '',
        passWord: '',
        dateOfBirth: this.startDate,
        sex: '',
        profilePicture: []
      };
      _form.resetForm();
    }

  }

  eMail = new FormControl('', [Validators.required, Validators.email]);

  getMailError() {
    return this.eMail.hasError('required') ? 'You must enter a value' :
      this.eMail.hasError('eMail') ? 'Not a valid email' : '';
  }

  OnSubmit(_RegForm: NgForm) {
    this.formHasErrors = "";
    if (_RegForm.valid) {
      this._usrService
        .registerNewUser(_RegForm.value)
        .subscribe((data: any) => {
          if (data.Succeeded == true) {
            this._toastr.success( this.registerUser.UserName ,"Registration Complete !");
            this.resetFormFields(_RegForm);
          }
          else {
            this._toastr.error(data.Errors[0]);
            this.formHasErrors = "This form is not valid";
          }
        })
    }
    else {
      this.formHasErrors = "This form is not valid";
    }

  }

}
