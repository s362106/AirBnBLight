import { Component, OnInit } from '@angular/core';
import { IHouse } from '../houses/house';
import { Router } from '@angular/router';
import { HouseService } from '../houses/houses.service';

@Component({
  selector: 'app-housecard-component',
  templateUrl: './housecard.component.html',
  styleUrls: ['./housecard.component.css']
})

export class HousecardComponent implements OnInit {
  viewTitle: string = 'Grid';
  houses: IHouse[] = [];

  constructor(
    private _router: Router,
    private _houseService: HouseService) { }

  private _listFilter: string = '';
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    console.log('In setter:', value);
    this.filteredHouses = this.performFilter(value);
  }

  getHouses(): void {
    this._houseService.getHouses()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.houses = data;
        this.filteredHouses = this.houses;
      }
      );
  }

  filteredHouses: IHouse[] = this.houses;

  performFilter(filterBy: string): IHouse[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.houses.filter((house: IHouse) =>
      house.Title.toLocaleLowerCase().includes(filterBy));
  }

  ngOnInit(): void {
    this.getHouses();
  }
}
