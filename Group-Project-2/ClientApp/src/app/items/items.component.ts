import { Component, OnInit } from '@angular/core';
import { IItem } from './item';
import { Router } from '@angular/router';
import { ItemService } from './items.service';

@Component({
  selector: 'app-items-component',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})

export class ItemsComponent implements OnInit {
  viewTitle: string = 'Table';
  displayImage: boolean = true;
  items: IItem[] = [];

  constructor(
    private _router: Router,
    private _itemService: ItemService) { }

  private _listFilter: string = '';
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    console.log('In setter:', value);
    this.filteredItems = this.performFilter(value);
  }

  deleteItem(item: IItem): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${item.Name}"?`);
    if (confirmDelete) {
      this._itemService.deleteItem(item.ItemId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this.filteredItems = this.filteredItems.filter(i => i !== item);
            }
          },
          (error) => {
            console.error('Error deleting item:', error);
          });
    }
  }

  getItems(): void {
    this._itemService.getItems()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.items = data;
        this.filteredItems = this.items;
      }
      );
  }

  filteredItems: IItem[] = this.items;

  performFilter(filterBy: string): IItem[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.items.filter((item: IItem) =>
      item.Name.toLocaleLowerCase().includes(filterBy));
  }

  toggleImage(): void {
    this.displayImage = !this.displayImage;
  }

  navigateToItemform() {
    this._router.navigate(['/itemform']);
  }

  ngOnInit(): void {
    this.getItems();
  }
}
