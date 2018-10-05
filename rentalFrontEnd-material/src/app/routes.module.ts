/* Routes Array Type */
import { Routes , RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

/* Guards */
import { AuthGuard } from "./guards/auth.guard";
import { CustomerRoleGuard } from "./guards/customer-role.guard";
import { EmployeeRoleGuard } from "./guards/employee-role.guard";
import { AdminRoleGuard } from "./guards/admin-role.guard";

/*Guest Components*/
import { MainComponent } from './layout/guest/main/main.component';
import { LoginComponent } from './layout/guest/login/login.component';
import { RegisterComponent } from './layout/guest/register/register.component';
import { SearchComponent } from './layout/guest/search/search.component';
import { SearchBarComponent } from "./layout/guest/search/car-list/search-bar/search-bar.component";
import { CarListComponent } from "./layout/guest/search/car-list/car-list.component";

/* Customer Components */
import { CustomerRentalsComponent } from './layout/customer/customer-rentals/customer-rentals.component';
import { RentVehicleCheckoutComponentComponent } from './layout/shared/rent-vehicle-checkout-component/rent-vehicle-checkout-component.component';

/* Employee Components */
import { EmployeeReturnVehicleComponent } from './layout/employee/employee-return-vehicle/employee-return-vehicle.component';
import { RentalResultsComponent } from './layout/employee/employee-return-vehicle/rental-results/rental-results.component';

/* Admin Components */
import { PanelComponent } from './layout/admin/panel/panel.component';
import { UserEditorComponent } from './layout/admin/panel/user-editor/user-editor.component';
import { VehicleEditorComponent } from './layout/admin/panel/vehicle-editor/vehicle-editor.component';
import { RentalEditorComponent } from './layout/admin/panel/rental-editor/rental-editor.component';

/* Shared Components */
import { PagenotfoundComponent } from "./layout/shared/pagenotfound/pagenotfound.component";
import { VehicleDetailsComponent } from './layout/shared/vehicle-details/vehicle-details.component';
import { RentSumCalculatorComponent } from './layout/shared/vehicle-details/rent-sum-calculator/rent-sum-calculator.component';
import { CheckoutChooseBoxComponent } from './layout/shared/rent-vehicle-checkout-component/checkout-choose-box/checkout-choose-box.component';
import { LastViewedVehiclesComponent } from './layout/shared/last-viewed-vehicles/last-viewed-vehicles.component';




const appRoutes: Routes = [
    { path: 'main', component: MainComponent },
    /* Public Routes */
    { path: 'vehicles', component: SearchComponent, data: { title: 'Vehicle List' }} ,
    { path: 'login', component: LoginComponent, data: { title: 'Login' } },
    { path: 'register', component: RegisterComponent, data: { title: 'Register' } },
    { path: 'vehicle/:id', component: VehicleDetailsComponent, data: { title: 'Vehicle Details' } },
    /* Customer Routes */
    { path: 'customer_rentals', component: CustomerRentalsComponent, data: { title: 'Customer Rentals' } ,canActivate:[AuthGuard,CustomerRoleGuard]},
    { path: 'CheckOut/:id', component: RentVehicleCheckoutComponentComponent, data: { title: 'CheckOut' } ,canActivate:[AuthGuard,CustomerRoleGuard]},
    /* Employee Routes */
    { path: 'return_rentals', component: EmployeeReturnVehicleComponent, data: { title: 'Return Rentals' } ,canActivate:[AuthGuard,EmployeeRoleGuard]},

    /* Admin Routes */
    { path: 'panel', component: PanelComponent, data: { title: 'Panel' } ,canActivate:[AuthGuard,AdminRoleGuard]},

    /* Default Routes */
    { path: '', redirectTo: '/main', pathMatch: 'full' },
    { path: '**', component: PagenotfoundComponent }];

@NgModule({
    imports: [
        RouterModule.forRoot( appRoutes/*,{ enableTracing: true }  <-- debugging purposes only)*/),
    ],
    exports: [
        RouterModule
    ]
  })

  export class RoutingModule{}
  export const routingComponents = [
    MainComponent,
    LoginComponent,
    RegisterComponent,
    SearchComponent,
    SearchBarComponent,
    CarListComponent,
    VehicleDetailsComponent,
    CustomerRentalsComponent,
    EmployeeReturnVehicleComponent,
    RentSumCalculatorComponent,
    RentVehicleCheckoutComponentComponent,
    CheckoutChooseBoxComponent,
    RentalResultsComponent,
    PanelComponent,
    UserEditorComponent,
    VehicleEditorComponent,
    RentalEditorComponent,
    LastViewedVehiclesComponent,
    PagenotfoundComponent
    ];

