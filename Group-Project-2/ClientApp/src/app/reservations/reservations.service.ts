import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IReservation } from './reservation';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  private baseUrl = 'api/reservation/';

  constructor(private _http: HttpClient) { }

  getReservations(): Observable<IReservation[]> {
    return this._http.get<IReservation[]>(this.baseUrl);
  }

  createReservation(newReservation: IReservation): Observable<any> {
    const createUrl = 'api/reservation/create';
    return this._http.post<any>(createUrl, newReservation);
  }

  getReservationById(reservationId: number): Observable<any> {
    const url = `${this.baseUrl}/${reservationId}`;
    return this._http.get(url);
  }

  updateReservation(reservationId: number, newReservation: any): Observable<any> {
    const url = `${this.baseUrl}/update/${reservationId}`;
    newReservation.reservationId = reservationId;
    return this._http.put<any>(url, newReservation);
  }

  deleteReservation(reservationId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${reservationId}`;
    return this._http.delete(url);
  }
}
