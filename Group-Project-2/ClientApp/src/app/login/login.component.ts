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
      this.authService.login(credentials)
      .subscribe(response => {
        if(response.success) {
          localStorage.setItem("userEmail", response.userEmail);
          this.authService.setStatus(true);
          this.router.navigate(["/houses"]);
          console.log("userEmail: ", response.userEmail);
        }
        console.log("Response: ", response.message);
      });
    }
}

