import { Component, OnInit } from '@angular/core';
import { IHouse } from '../houses/house';
import { ActivatedRoute, Router } from '@angular/router';
import { HouseService } from '../houses/houses.service';

@Component({
  selector: 'app-house-details-component',
  templateUrl: './house-details.component.html',
  styleUrls: ['./house-details.component.css']
})

export class HouseDetailsComponent implements OnInit {
  viewTitle: string = 'Details';
  house!: IHouse;

  constructor(
    private _router: Router,
    private _houseService: HouseService,
    private activatedRoute: ActivatedRoute) {
    activatedRoute.params.subscribe((params) => {
      if (params.id)
        this.loadHouse(+params['id'])
    })
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

  ngOnInit(): void {

  }
}
