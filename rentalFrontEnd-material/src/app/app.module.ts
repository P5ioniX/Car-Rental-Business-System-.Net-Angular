import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule , ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from '@angular/http';

/* Components */
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbars/navbar/navbar.component';
import { CustomerNavbarComponent } from './navbars/customer-navbar/customer-navbar.component';
import { EmployeeNavbarComponent } from './navbars/employee-navbar/employee-navbar.component';
import { AdminNavbarComponent } from './navbars/admin-navbar/admin-navbar.component';
import { AddVehicleDialog, VehicleEditorComponent } from './layout/admin/panel/vehicle-editor/vehicle-editor.component';

/* Material */
import { MaterialModule } from './material.module';
import { ShowOnDirtyErrorStateMatcher, ErrorStateMatcher, MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

/* Modules ForRoot */
import { AppRoutingModule } from './app-routing.module';
import { ToastrModule } from "ngx-toastr";

/*Grouped Routes File*/
import { routingComponents, RoutingModule } from "./routes.module";

/* Guards */
import { AuthGuard } from './guards/auth.guard';
import { CustomerRoleGuard } from './guards/customer-role.guard';
import { EmployeeRoleGuard } from './guards/employee-role.guard';
import { AdminRoleGuard } from './guards/admin-role.guard';

/* Services */
import { SearchBarService } from './services/search-bar.service';
import { UserServise } from './services/user.service';
import { RentalService } from './services/rental.service';


import { FirstHttpInterceptor } from './guards/first-http-interceptor';
import { HttpClientModule ,HttpClient, HTTP_INTERCEPTORS} from '@angular/common/http';
import { AuthService } from './services/auth.service';





@NgModule({
  declarations: [
    AppComponent,
    AddVehicleDialog,
    routingComponents,// <-- Grouped Routes File
    NavbarComponent,
    CustomerNavbarComponent,
    EmployeeNavbarComponent,
    AdminNavbarComponent
],
  imports: [
    RoutingModule,
    ToastrModule.forRoot({positionClass: "toast-bottom-center"}),
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    ReactiveFormsModule,
    MaterialModule, // <-- Materialize Grouped Module
    BrowserAnimationsModule,
  ],
  entryComponents: [
    AddVehicleDialog,VehicleEditorComponent
  ],
  providers: [
             {provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher},
             {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: true}},//<-------- Material Dialog
             {provide: HTTP_INTERCEPTORS , useClass:FirstHttpInterceptor,multi:true}, //<-------- Http Interceptor Object
             AuthGuard,CustomerRoleGuard,EmployeeRoleGuard,AdminRoleGuard, //<-------- Guards
             UserServise,SearchBarService,RentalService,AuthService //<-------- Services
            ],
  bootstrap: [AppComponent]
})
export class AppModule { }
