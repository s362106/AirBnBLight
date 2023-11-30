import { Component } from '@angular/core';
import { AuthService } from '../authentication/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isLoggedIn: boolean = false;

    constructor(private authService: AuthService, private router: Router) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  isAuthenticated() {
    return this.authService.isAuthenticated();
  }

  logout() {
    localStorage.clear();
    this.authService.logout()
      .subscribe(response => {
          if (response.success) {
            this.router.navigate(["/login"]);
            console.log("Response: ", response.message);
        }
      })
  }
}
