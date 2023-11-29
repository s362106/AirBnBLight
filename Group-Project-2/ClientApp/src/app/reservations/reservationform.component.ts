import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservationService } from './reservations.service';
import { HouseService } from '../houses/houses.service';
import { IHouse } from '../houses/house';

@Component({
  selector: "app-reservations-reservationform",
  templateUrl: "./reservationform.component.html",
  styleUrls: ['./reservationform.component.css']
})
export class ReservationformComponent {
  reservationForm: FormGroup;
  isEditMode: boolean = false;
  isCreateMode: boolean = false;
  reservationId: number = -1;
  houses: IHouse[] = [];
  numberOfDays: number = 0;
  totalPrice: number = 0;
  currentDate: string = '';
  checkIn: string = '';
  checkOut: string = this.checkIn;

  constructor(
    private _formbuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    private _reservationService: ReservationService,
    private _houseService: HouseService
  ) {
    this.reservationForm = _formbuilder.group({
      houseId: ['', Validators.required],
      checkInDate: [this.formatDate(new Date()), Validators.required],
      checkOutDate: [this.formatDate(new Date()), Validators.required],
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
            window.location.reload();
          } else {
            console.log('Reservation update failed');
          }
        });
    } else {
      this._reservationService.createReservation(newReservation)
        .subscribe(response => {
          if (response.success) {
            console.log(response.message);
            window.location.reload();
          }
          else {
            console.log('Reservation creation failed');
          }
        });
    }
  }

  backToReservations() {
    this._route.params.subscribe(params => {
      if (params['view'] === 'Table') {
        this._router.navigate(['/reservations']);
      } else if (params['view'] === 'Details') {
        this._router.navigate(['/reservation-details/' + this.reservationId]);
      }
    });
  }

  calculateNumberOfDays() {
    const checkInDateStr = this.reservationForm?.get('checkInDate')?.value;
    const checkOutDateStr = this.reservationForm?.get('checkOutDate')?.value;

    if (checkInDateStr && checkOutDateStr) {
      const checkInDate = new Date(checkInDateStr);
      const checkOutDate = new Date(checkOutDateStr);

      if (!isNaN(checkInDate.getTime()) && !isNaN(checkOutDate.getTime())) {
        const timeDiff = checkOutDate.getTime() - checkInDate.getTime();
        const numberOfDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        this.numberOfDays = numberOfDays;
        this.totalPrice = this.chosenHouse?.PricePerNight * numberOfDays;
      } else {
        console.error('Invalid date strings');
      }
    } else {
      console.error('Invalid date strings');
    }
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  loadReservationForEdit(reservationId: number) {
    this._reservationService.getReservationById(reservationId)
      .subscribe(
        (reservation: any) => {
          console.log('retrieved reservation: ', reservation);
          this.reservationForm.patchValue({
            houseId: reservation.HouseId,
            checkInDate: this.formatDate(new Date(reservation.CheckInDate)),
            checkOutDate: this.formatDate(new Date(reservation.CheckOutDate)),
          });
          this.changeHouse(reservation.HouseId);
          this.totalPrice = reservation.TotalPrice;
          this.checkIn = reservation.CheckInDate;
          console.log('Total price: ' + this.totalPrice);
        }, (error: any) => {
          console.error('Error loading reservation for edit:', error);
        }
      );
  }

  chosenHouse!: IHouse;
  changeHouse(houseIdString: string) {
    const houseId: number = +houseIdString;
    if (!isNaN(houseId)) {
      this._houseService.getHouseById(houseId)
        .subscribe(
          (house: any) => {
            this.chosenHouse = house;
          }, (error: any) => {
            console.error('Error changing houses:', error);
          }
        );
    } else {
      console.error('houseSelect is not a number');
    }
  }

  ngOnInit(): void {
    this.getHouses();
    this._route.params.subscribe(params => {
      if (params['mode'] === 'create') {
        this.isEditMode = false; 
        this.isCreateMode = true;
      } else if (params['mode'] === 'edit') {
        this.isEditMode = true; 
        this.isCreateMode = false;
        this.reservationId = +params['id']; 
        this.loadReservationForEdit(this.reservationId);
      }
    });
    this.currentDate = this.formatDate(new Date());
    this.checkIn = this.currentDate;
    this.reservationForm.get('checkInDate')?.valueChanges.subscribe(() => {
      this.calculateNumberOfDays();
      this.checkIn = this.reservationForm?.get('checkInDate')?.value;
      const checkInDate = this.reservationForm?.get('checkInDate')?.value as Date;
      const checkOutDate = this.reservationForm?.get('checkOutDate')?.value as Date;
      if (checkOutDate < checkInDate) {
        this.reservationForm.get('checkOutDate')?.setValue(this.checkIn);
      }
    });

    this.reservationForm.get('checkOutDate')?.valueChanges.subscribe(() => {
      this.calculateNumberOfDays();
      this.checkOut = this.reservationForm.get('checkOutDate')?.value;
    });
  }
}
