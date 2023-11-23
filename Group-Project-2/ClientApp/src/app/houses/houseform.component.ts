import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HouseService } from './houses.service';

@Component({
  selector: "app-houses-houseform",
  templateUrl: "./houseform.component.html"
})
export class HouseformComponent {
  houseForm: FormGroup;
  isEditMode: boolean = false;
  houseId: number = -1;

  constructor(
    private _formbuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    private _houseService: HouseService
  ) {
    this.houseForm = _formbuilder.group({
      title: ['', Validators.required],
      location: [''],
      pricePerNight: [0, Validators.required],
      bedrroms: [0, Validators.required],
      bathrooms: [0, Validators.required],
      description: [''],
      houseImageUrl: [''],
      bedroomImageUrl: [''],
      bathroomImageUrl: ['']
    });
  }

  onSubmit() {
    console.log("HouseCreate form submitted:");
    console.log(this.houseForm);
    const newHouse = this.houseForm.value;
    if (this.isEditMode) {
      this._houseService.updateHouse(this.houseId, newHouse)
        .subscribe(response => {
          if (response.success) {
            console.log(response.message);
            this._router.navigate(['/houses']);
          } else {
            console.log('House update failed');
          }
        });
    } else {
      this._houseService.createHouse(newHouse)
        .subscribe(response => {
          if (response.success) {
            console.log(response.message);
            this._router.navigate(['/houses']);
          }
          else {
            console.log('House creation failed');
          }
        });
    }
  }

  backToHouses() {
    this._router.navigate(['/houses']);
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      if (params['mode'] === 'create') {
        this.isEditMode = false; // Create mode
      } else if (params['mode'] === 'edit') {
        this.isEditMode = true; // Edit mode
        this.houseId = +params['id']; // Convert to number
        this.loadHouseForEdit(this.houseId);
      }
    });
  }

  loadHouseForEdit(houseId: number) {
    this._houseService.getHouseById(houseId)
      .subscribe(
        (house: any) => {
          console.log('retrieved house: ', house);
          this.houseForm.patchValue({
            title: house.Title,
            location: house.Location,
            pricePerNight: house.PricePerNight,
            bedrooms: house.Bedrooms,
            bathrooms: house.Title,
            description: house.Description,
            houseImageUrl: house.HouseImageUrl,
            bedroomImageUrl: house.BedroomImageUrl,
            bathroomImageUrl: house.BathroomImageUrl
          });
        }, (error: any) => {
          console.error('Error loading house for edit:', error);
        }
      );
  }
}

