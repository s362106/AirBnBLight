import { Component, OnInit } from '@angular/core';
import { IReservation } from './reservation';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservationService } from './reservations.service';
import { HouseService } from '../houses/houses.service';

@Component({
  selector: 'app-reservations-component',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})

export class ReservationsComponent implements OnInit {
  viewTitle: string = 'Table';
  displayImage: boolean = true;
  reservations: IReservation[] = [];

  constructor(
    private _router: Router,
    private _reservationService: ReservationService,
    private _houseService: HouseService,
    private route: ActivatedRoute) { }

  private _listFilter: string = '';
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    console.log('In setter:', value);
    this.filteredReservations = this.performFilter(value);
  }
  

  deleteReservation(reservation: IReservation): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${reservation.ReservationId}"?`);
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

  getReservations(): void {
    this._reservationService.getReservations()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.reservations = data;
        this.filteredReservations = this.reservations;
        this.setHouseForReservations();
      }
      );
  }

  filteredReservations: IReservation[] = this.reservations;

  setHouseForReservations() {
    this.reservations.forEach(reservation => {
      this._houseService.getHouseById(reservation.HouseId).subscribe(house => {
        reservation.House = house;
      });
    });
  }

  performFilter(filterBy: string): IReservation[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.reservations.filter((reservation: IReservation) =>
      reservation.House?.Title.toLocaleLowerCase().includes(filterBy));
  }

  toggleImage(): void {
    this.displayImage = !this.displayImage;
  }

  navigateToReservationform() {
    this._router.navigate(['/reservationform']);
  }

  ngOnInit(): void {
    this.getReservations();
    this.route.params.subscribe(data => {
      this.getReservations();
    });
  }
}
