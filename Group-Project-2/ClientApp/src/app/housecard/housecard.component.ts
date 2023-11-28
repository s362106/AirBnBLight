import { Component, OnInit } from '@angular/core';
import { IHouse } from '../houses/house';
import { Router } from '@angular/router';
import { HouseService } from '../houses/houses.service';

@Component({
  selector: 'app-housecard-component',
  templateUrl: './housecard.component.html'
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

  deleteHouse(house: IHouse): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${house.Title}"?`);
    if (confirmDelete) {
      this._houseService.deleteHouse(house.HouseId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this.filteredHouses = this.filteredHouses.filter(i => i !== house);
            }
          },
          (error) => {
            console.error('Error deleting house:', error);
          });
    }
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
