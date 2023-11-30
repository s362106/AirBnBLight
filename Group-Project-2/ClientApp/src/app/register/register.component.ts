import { Component } from '@angular/core';
import { AuthService } from '../authentication/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  matchingError: boolean = false;

  constructor(private formBuilder: FormBuilder, private router: Router, private authService: AuthService) {
    this.registerForm = formBuilder.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      PhoneNumber: ['', Validators.required],
      Password: ['', [Validators.required, Validators.min(8)]],
      ConfirmPassword: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
    const providedPassword = this.registerForm.get("Password")?.value;
    const confirmPassword = this.registerForm.get("ConfirmPassword")?.value;

    if(providedPassword !== confirmPassword) {
      console.log("Passwords do not match");
      this.matchingError = true;
      return;
    }
    console.log("Register form submitted");
    console.log("Register Data: ", JSON.stringify(this.registerForm.value));
    const info = this.registerForm.value;
    this.authService.register(info)
    .subscribe((response) => {
      if(response.success) {
        console.log("Registration success. User: ", JSON.stringify(info));
        this.router.navigate(['/login'])
      }
      else {
        console.log("Registration failed. Error: ", response.message);
      }
    })
  }
}

