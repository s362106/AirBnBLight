import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { HousesComponent } from './houses/houses.component';
import { ConvertToCurrency } from './shared/convert-to-currency.pipe';
import { HouseformComponent } from './houses/houseform.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { ReservationformComponent } from './reservations/reservationform.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HousesComponent,
    ConvertToCurrency,
    HouseformComponent,
    ReservationsComponent,
    ReservationformComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'houses', component: HousesComponent },
      { path: 'reservations', component: ReservationsComponent },
      { path: 'houseform', component: HouseformComponent },
      { path: 'reservationform', component: ReservationformComponent },
      { path: 'houseform/:mode/:id', component: HouseformComponent },
      { path: 'reservationform/:mode/:id', component: ReservationformComponent },
      { path: '**', redirectTo: '', pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

