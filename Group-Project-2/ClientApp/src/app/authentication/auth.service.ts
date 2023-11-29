import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILoginModel } from '../login/logInModel';
import { BehaviorSubject, Observable, Subject, catchError, throwError, tap, of } from 'rxjs';
import { Router } from '@angular/router';
import { RegisterModel } from '../register/registerModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = "api/user";
  private isLoggedIn = false;
  
  constructor(private http: HttpClient, private router: Router) { }

  public login(user: ILoginModel): Observable<any> {
    const url = `${this.baseUrl}/login`;
    return this.http.post(url, user).pipe(
      tap(() => {
        this.isLoggedIn = true;
      }),
      catchError((error) => {
        console.error('Login failed', error);
        return of(false); 
      })
    );
  }

  public logout(): Observable<any> {
    const url = `${this.baseUrl}/logout`;
    this.isLoggedIn = false;
    this.router.navigate(['/login']);
    return this.http.post(url, "");
  }

  isAuthenticated(): boolean {
    return this.isLoggedIn;
  }

  public register(user: RegisterModel): Observable<any> {
    console.log(JSON.stringify(user));
    const url = `/${this.baseUrl}/register`;
    return this.http.post(url, user);
  }
}
