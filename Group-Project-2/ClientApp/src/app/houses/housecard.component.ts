import { Component, OnInit } from '@angular/core';
import { IHouse } from './house';
import { Router } from '@angular/router';
import { HouseService } from './houses.service';

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

  ngOnInit(): void {
    this._houseService.getHouses()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.houses = data;
      });
  }

}
