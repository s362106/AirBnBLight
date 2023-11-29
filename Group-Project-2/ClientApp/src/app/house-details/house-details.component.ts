import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { IHouse } from '../houses/house';
import { ActivatedRoute, Router } from '@angular/router';
import { HouseService } from '../houses/houses.service';
import { ReservationService } from '../reservations/reservations.service';

@Component({
  selector: 'app-house-details-component',
  templateUrl: './house-details.component.html',
  styleUrls: ['./house-details.component.css']
})

export class HouseDetailsComponent implements OnInit {
  reservationForm: FormGroup;
  viewTitle: string = 'Details';
  house!: IHouse;

  constructor(
    private _formbuilder: FormBuilder,
    private _router: Router,
    private _reservationService: ReservationService,
    private _houseService: HouseService,
    private activatedRoute: ActivatedRoute)
  {
    activatedRoute.params.subscribe((params) => {
      if (params.id)
        this.loadHouse(+params['id'])
    })
    this.reservationForm = _formbuilder.group({
      checkInDate: [this.formatDate(new Date())],
      checkOutDate: [this.formatDate(new Date())],
    });
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  deleteHouse(house: IHouse): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${house.Title}"?`);
    if (confirmDelete) {
      this._houseService.deleteHouse(house.HouseId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
            }
          },
          (error) => {
            console.error('Error deleting house:', error);
          });
    }
  }

  loadHouse(houseId: number) {
    this._houseService.getHouseById(houseId)
      .subscribe(
        (house: any) => {
          console.log('retrieved house: ', house);
          this.house = house;
        }, (error: any) => {
          console.error('Error loading house for details view:', error);
        }
      );
  }

  onSubmit() {
    console.log("ReservationCreate form submitted:");
    console.log(this.reservationForm);
    const houseId = this.house.HouseId;
    const newReservation = { houseId, ...this.reservationForm.value };
    this._reservationService.createReservation(newReservation)
      .subscribe(response => {
        if (response.success) {
          console.log(response.message);
          this._router.navigate(['/reservations']);
        }
        else {
          console.log('Reservation creation failed');
        }
      });
  }

  backToHouses() {
    this._router.navigate(['/houses']);
  }

  ngOnInit(): void {

  }
}
