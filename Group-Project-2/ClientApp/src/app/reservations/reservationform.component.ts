import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservationService } from './reservations.service';
import { HouseService } from '../houses/houses.service';
import { IHouse } from '../houses/house';

@Component({
  selector: "app-reservations-reservationform",
  templateUrl: "./reservationform.component.html"
})
export class ReservationformComponent {
  reservationForm: FormGroup;
  isEditMode: boolean = false;
  reservationId: number = -1;
  houses: IHouse[] = [];

  constructor(
    private _formbuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    private _reservationService: ReservationService,
    private _houseService: HouseService
  ) {
    this.reservationForm = _formbuilder.group({
      houseId: ['', Validators.required],
      checkInDate: [new Date(), Validators.required],
      checkOutDate: [new Date(), Validators.required],
    });
  }

  getHouses(): void {
    this._houseService.getHouses()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.houseSelectList = data;
      }
      );
  }

  houseSelectList: IHouse[] = this.houses;

  onSubmit() {
    console.log("ReservationCreate form submitted:");
    console.log(this.reservationForm);
    const newReservation = this.reservationForm.value;
    if (this.isEditMode) {
      this._reservationService.updateReservation(this.reservationId, newReservation)
        .subscribe(response => {
          if (response.success) {
            console.log(response.message);
            this._router.navigate(['/reservations']);
          } else {
            console.log('Reservation update failed');
          }
        });
    } else {
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
  }

  backToReservations() {
    this._router.navigate(['/reservations']);
  }

  ngOnInit(): void {
    this.getHouses();
    this._route.params.subscribe(params => {
      if (params['mode'] === 'create') {
        this.isEditMode = false; // Create mode
      } else if (params['mode'] === 'edit') {
        this.isEditMode = true; // Edit mode
        this.reservationId = +params['id']; // Convert to number
        this.loadReservationForEdit(this.reservationId);
      }
    });
  }

  loadReservationForEdit(reservationId: number) {
    this._reservationService.getReservationById(reservationId)
      .subscribe(
        (reservation: any) => {
          console.log('retrieved reservation: ', reservation);
          this.reservationForm.patchValue({
            houseId: reservation.HouseId,
            checkInDate: reservation.CheckInDate,
            checkOutDate: reservation.CheckOutDate,
          });
        }, (error: any) => {
          console.error('Error loading reservation for edit:', error);
        }
      );
  }
}

