import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemService } from './items.service';

@Component({
  selector: "app-items-itemform",
  templateUrl: "./itemform.component.html"
})
export class ItemformComponent {
  itemForm: FormGroup;
  isEditMode: boolean = false;
  itemId: number = -1;

  constructor(
    private _formbuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    private _itemService: ItemService
  ) {
    this.itemForm = _formbuilder.group({
      name: ['', Validators.required],
      price: [0, Validators.required],
      description: [''],
      imageUrl: ['']
    });
  }

  onSubmit() {
    console.log("ItemCreate form submitted:");
    console.log(this.itemForm);
    const newItem = this.itemForm.value;
    if (this.isEditMode) {
      this._itemService.updateItem(this.itemId, newItem)
        .subscribe(response => {
          if (response.success) {
            console.log(response.message);
            this._router.navigate(['/items']);
          } else {
            console.log('Item update failed');
          }
        });
    } else {
      this._itemService.createItem(newItem)
        .subscribe(response => {
          if (response.success) {
            console.log(response.message);
            this._router.navigate(['/items']);
          }
          else {
            console.log('Item creation failed');
          }
        });
    }
  }

  backToItems() {
    this._router.navigate(['/items']);
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      if (params['mode'] === 'create') {
        this.isEditMode = false; // Create mode
      } else if (params['mode'] === 'edit') {
        this.isEditMode = true; // Edit mode
        this.itemId = +params['id']; // Convert to number
        this.loadItemForEdit(this.itemId);
      }
    });
  }

  loadItemForEdit(itemId: number) {
    this._itemService.getItemById(itemId)
      .subscribe(
        (item: any) => {
          console.log('retrived item: ', item);
          this.itemForm.patchValue({
            name: item.Name,
            price: item.Price,
            description: item.Description,
            imageUrl: item.ImageUrl
          });
        }, (error: any) => {
          console.error('Error loading item for edit:', error);
        }
      );
  }
}

