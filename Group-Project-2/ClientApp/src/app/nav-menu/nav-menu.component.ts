import { Component } from '@angular/core';
import { AuthService } from '../authentication/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private authService: AuthService) { }

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
          console.log("Response: ", response.message);
        }
      })
  }
}
