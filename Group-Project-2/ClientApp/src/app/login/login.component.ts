import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = "";

  constructor(private _formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService) {
      this.loginForm = _formBuilder.group( {
        Email: ['', Validators.required],
        ProvidedPassword: ['', Validators.required]
      });
    }

  onSubmit() {
    console.log("Login form submitted");
    console.log(this.loginForm);

    const credentials = this.loginForm.value;
    this.authService.login(credentials).subscribe(
      (response) => {
        if (response.success) {
          localStorage.setItem("userEmail", response.userEmail);
          this.router.navigate(["/"]);
          console.log("userEmail: ", response.userEmail);
        } else {
          this.errorMessage = "Invalid email or password. Try Again.";
          console.log("Error message:", this.errorMessage);
        }
      },
      (error) => {
        console.error("Error during login:", error);
        this.errorMessage = "An unexpected error occurred.";
      }
    );
  }

}
