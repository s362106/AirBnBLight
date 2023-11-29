import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservationService } from '../reservations/reservations.service';
import { IReservation } from '../reservations/reservation';
import { HouseService } from '../houses/houses.service';

@Component({
  selector: 'app-reservation-details-component',
  templateUrl: './reservation-details.component.html',
  styleUrls: ['./reservation-details.component.css']
})

export class ReservationDetailsComponent implements OnInit {
  viewTitle: string = 'Details';
  reservation!: IReservation;

  constructor(
    private _router: Router,
    private _reservationService: ReservationService,
    private _houseService: HouseService,
    private activatedRoute: ActivatedRoute) {
    activatedRoute.params.subscribe((params) => {
      if (params.id)
        this.loadReservation(+params['id'])
    })
  }

  deleteReservation(reservation: IReservation): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${reservation.House?.Title}"?`);
    if (confirmDelete) {
      this._reservationService.deleteReservation(reservation.ReservationId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this._router.navigate(['/reservations']);
            }
          },
          (error) => {
            console.error('Error deleting reservation:', error);
          });
    }
  }

  loadReservation(reservationId: number) {
    this._reservationService.getReservationById(reservationId)
      .subscribe(
        (reservation: any) => {
          console.log('retrieved reservation: ', reservation);
          this.reservation = reservation;
        }, (error: any) => {
          console.error('Error loading reservation for details view:', error);
        }
      );
  }

  setHouseForReservations() {
    this._houseService.getHouseById(this.reservation.HouseId).subscribe(house => {
      console.log('Retrieved house:', house);
      this.reservation.House = house;
      console.log('Reservation after setting House:', this.reservation);
    });
  }

  backToReservations() {
    this._router.navigate(['/reservations']);
  }

  ngOnInit(): void {

  }
}
