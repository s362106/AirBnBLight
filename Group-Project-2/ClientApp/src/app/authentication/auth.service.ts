import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILoginModel } from '../login/logInModel';
import { BehaviorSubject, Observable, Subject, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { RegisterModel } from '../register/registerModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = "api/user";
  
  constructor(private http: HttpClient, private router: Router) { }

  public login(user: ILoginModel): Observable<any> {
    const url = `${this.baseUrl}/login`;
    return this.http.post(url, user);
  }

  public logout(): Observable<any> {
    const url = `${this.baseUrl}/logout`;
    return this.http.post(url, "");
  }

  public register(user: RegisterModel): Observable<any> {
    console.log(JSON.stringify(user));
    const url = `/${this.baseUrl}/register`;
    return this.http.post(url, user);
  }
}