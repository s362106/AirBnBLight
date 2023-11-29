import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IHouse } from './house';

@Injectable({
  providedIn: 'root'
})
export class HouseService {

  private baseUrl = 'api/house/';

  constructor(private _http: HttpClient) { }

  getHouses(): Observable<IHouse[]> {
    return this._http.get<IHouse[]>(this.baseUrl);
  }

  createHouse(newHouse: IHouse): Observable<any> {
    const createUrl = 'api/house/create';
    return this._http.post<any>(createUrl, newHouse);
  }

  getHouseById(houseId: number): Observable<any> {
    const url = `${this.baseUrl}/${houseId}`;
    return this._http.get(url);
  }

  updateHouse(houseId: number, newHouse: any): Observable<any> {
    const url = `${this.baseUrl}/update`;
    newHouse.houseId = houseId;
    return this._http.post<any>(url, newHouse);
  }

  deleteHouse(houseId: number): Observable<any> {
    const url = `${this.baseUrl}/${houseId}`;
    return this._http.delete(url);
  }
}
