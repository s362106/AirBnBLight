import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IItem } from './item'; // Import your item model

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  private baseUrl = 'api/item/';

  constructor(private _http: HttpClient) { }

  getItems(): Observable<IItem[]> {
    return this._http.get<IItem[]>(this.baseUrl);
  }

  createItem(newItem: IItem): Observable<any> {
    const createUrl = 'api/item/create';
    return this._http.post<any>(createUrl, newItem);
  }

  getItemById(itemId: number): Observable<any> {
    const url = `${this.baseUrl}/${itemId}`;
    return this._http.get(url);
  }

  updateItem(itemId: number, newItem: any): Observable<any> {
    const url = `${this.baseUrl}/update/${itemId}`;
    newItem.itemId = itemId;
    return this._http.put<any>(url, newItem);
  }

  deleteItem(itemId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${itemId}`;
    return this._http.delete(url);
  }
}
