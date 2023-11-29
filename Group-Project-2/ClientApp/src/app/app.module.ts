import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
  
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ConvertToCurrency } from './shared/convert-to-currency.pipe';
import { HousesComponent } from './houses/houses.component';
import { HouseformComponent } from './houses/houseform.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { ReservationformComponent } from './reservations/reservationform.component';
import { HouseDetailsComponent } from './house-details/house-details.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuardService } from './authentication/authguard.service';
import { ReservationDetailsComponent } from './reservation-details/reservation-details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HousesComponent,
    ConvertToCurrency,
    HouseformComponent,
    ReservationsComponent,
    ReservationformComponent,
    HouseDetailsComponent,
    LoginComponent,
    RegisterComponent,
    ReservationDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'houses', component: HousesComponent },
      { path: 'reservations', component: ReservationsComponent, canActivate: [AuthGuardService] },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'houseform', component: HouseformComponent },
      { path: 'reservationform', component: ReservationformComponent },
      { path: 'house-details/:id', component: HouseDetailsComponent },
      { path: 'reservation-details/:id', component: ReservationDetailsComponent },
      { path: 'houseform/:mode/:id/:view', component: HouseformComponent, canActivate: [AuthGuardService] },
      { path: 'reservationform/:mode/:id/:view', component: ReservationformComponent, canActivate: [AuthGuardService] },
      { path: '**', redirectTo: '', pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }

