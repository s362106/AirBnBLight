import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HouseService } from './houses.service';

@Component({
  selector: "app-houseform-component",
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
      title: ['', [Validators.required, Validators.pattern('[0-9a-zA-ZæøåÆØÅ. \\-]{2,50}')]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      houseImageUrl: ['', [Validators.required, Validators.pattern('[https?://\\S+|www\\.\\S+]{10,300}')]],
      bedroomImageUrl: ['', [Validators.pattern('[https?://\\S+|www\\.\\S+]{10,300}')]],
      bathroomImageUrl: ['', [Validators.pattern('[https?://\\S+|www\\.\\S+]{10,300}')]],
      location: ['', [Validators.required, Validators.pattern('^[A-ZÆØÅ][a-zAæøå]{1,25},\\s[A-ZÆØÅ][a-zA-Zæøå]{1,25}$')]],
      pricePerNight: [0, [Validators.required, Validators.min(0.01)]],
      bedrooms: [0, [Validators.required, Validators.min(0)]],
      bathrooms: [0, [Validators.required, Validators.min(0)]]
    });
  }

  onSubmit() {
    console.log("HouseCreate form submitted:");
    console.log(this.houseForm);
    const newHouse = this.houseForm.value;
    if (this.isEditMode) {
      console.log("HouseDetails: ", JSON.stringify(this.houseForm.value));
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
      console.log("HouseDetails: ", JSON.stringify(this.houseForm.value));
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
    this._route.params.subscribe(params => {
      if (params['view'] === 'Table' || params['view'] === 'Grid') {
        this._router.navigate(['/houses']);
      } else if (params['view'] === 'Details') {
        this._router.navigate(['/house-details/' + this.houseId]);
      }
    });
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
            bathrooms: house.Bathrooms,
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

